using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Reflection;
using TaskManager.Entity;
using TaskManager.Repository;

namespace TaskManager.Service
{
    public class QuartzHelper
    {
        private static object obj = new object();

        private static IScheduler scheduler = null;

        /// <summary>
        /// 缓存任务所在程序集信息
        /// </summary>
        private static Dictionary<string, Assembly> AssemblyDict = new Dictionary<string, Assembly>();

        /// <summary>
        /// 初始化任务调度对象
        /// </summary>
        public static void InitScheduler()
        {
            try
            {
                lock (obj)
                {
                    if (scheduler == null)
                    {
                        //// 配置文件的方式，配置quartz实例
                        ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                        scheduler = schedulerFactory.GetScheduler();
                        scheduler.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("任务调度初始化失败！", ex);
            }
        }
        
        /// <summary>
        /// 启用任务调度
        /// 启动调度时会把任务表中状态为“执行中”的任务加入到任务调度队列中
        /// </summary>
        public static void StartScheduler()
        {
            try
            {
                //添加全局监听
                scheduler.ListenerManager.AddTriggerListener(new TriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());
                
                ///获取所有执行中的任务
                var jobList = new ScheduleJobRepository().GetScheduleJobList();
                if (jobList != null && jobList.Count > 0)
                {
                    foreach (var item in jobList)
                    {
                        try
                        {
                            if (item.JobStatus == (int)JobStatus.Run)
                            {
                                AddJob(item);
                            }
                        }
                        catch (Exception e)
                        { 
                            // 记录日志
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="jobModel"></param>
        public static void AddJob(ScheduleJobEntity jobModel)
        {
            //验证是否正确的Cron表达式
            if (ValidExpression(jobModel.CronExpression))
            {
                IJobDetail job = new JobDetailImpl(jobModel.JobKey, GetTypeByNew(jobModel.TypeString));

                CronTriggerImpl trigger = new CronTriggerImpl();
                trigger.CronExpressionString = jobModel.CronExpression;
                trigger.Name = jobModel.JobKey;
                //trigger.Description = jobModel.Description;
                scheduler.ScheduleJob(job, trigger);
            }
            else
            {
                throw new Exception(jobModel.CronExpression + "不是正确的Cron表达式,无法启动该任务!");
            }
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <param name="jobModel"></param>
        public static void UpdateJob(ScheduleJobEntity jobModel)
        {
            DeleteJob(jobModel.JobKey);

            //验证是否正确的Cron表达式
            if (ValidExpression(jobModel.CronExpression))
            {
                IJobDetail job = new JobDetailImpl(jobModel.JobKey, GetTypeByNew(jobModel.TypeString));

                CronTriggerImpl trigger = new CronTriggerImpl();
                trigger.CronExpressionString = jobModel.CronExpression;
                trigger.Name = jobModel.JobKey;
                //trigger.Description = jobModel.Description;
                scheduler.ScheduleJob(job, trigger);
            }
            else
            {
                throw new Exception(jobModel.CronExpression + "不是正确的Cron表达式,无法启动该任务!");
            }
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="JobKey"></param>
        public static void PauseJob(string JobKey)
        {
            try
            {
                JobKey jk = new JobKey(JobKey);
                if (scheduler.CheckExists(jk))
                {
                    //任务已经存在则暂停任务
                    scheduler.PauseJob(jk);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="JobKey">任务key</param>
        public static void ResumeJob(string JobKey)
        {
            try
            {
                JobKey jk = new JobKey(JobKey);
                if (scheduler.CheckExists(jk))
                {
                    //任务已经存在则暂停任务
                    scheduler.ResumeJob(jk);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        public static void StopSchedule()
        {
            try
            {
                //判断调度是否已经关闭
                if (!scheduler.IsShutdown)
                {
                    //等待任务运行完成
                    scheduler.Shutdown(true);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 删除现有任务
        /// </summary>
        /// <param name="JobKey"></param>
        public static void DeleteJob(string JobKey)
        {
            try
            {
                JobKey jk = new JobKey(JobKey);
                if (scheduler.CheckExists(jk))
                {
                    //任务已经存在则删除
                    scheduler.DeleteJob(jk);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Type GetTypeByNew(string typeString)
        {
            try
            {
                Type type = System.Type.GetType(typeString);
                return type;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        /// <summary>
        /// 校验字符串是否为正确的Cron表达式
        /// </summary>
        /// <param name="cronExpression">带校验表达式</param>
        /// <returns></returns>
        public static bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }


        /// <summary>
        /// 获取任务在未来周期内哪些时间会运行
        /// </summary>
        /// <param name="CronExpressionString">Cron表达式</param>
        /// <param name="numTimes">运行次数</param>
        /// <returns>运行时间段</returns>
        public static List<string> GetTaskeFireTime(string cronExpressionString, int numTimes)
        {
            var flag = ValidExpression(cronExpressionString);
            if (!flag)
            {
                return new List<string>();
            }
            if (numTimes < 0)
            {
                throw new Exception("参数numTimes值大于等于0");
            }
            //时间表达式
            ITrigger trigger = TriggerBuilder.Create().WithCronSchedule(cronExpressionString).Build();
            IList<DateTimeOffset> dates = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, numTimes);
            List<string> list = new List<string>();
            foreach (DateTimeOffset dtf in dates)
            {
                list.Add(TimeZoneInfo.ConvertTimeFromUtc(dtf.DateTime, TimeZoneInfo.Utc).ToString());
            }
            return list;
        }
    }
}
