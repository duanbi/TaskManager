using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entity;
using TaskManager.Repository;

namespace TaskManager.Service
{
    public class JobManagerService
    {
        private ScheduleJobRepository _scheduleJobRepository = new ScheduleJobRepository();

        /// <summary>
        /// 全部停止
        /// </summary>
        public void StopAll()
        {
            QuartzHelper.StopSchedule();
            _scheduleJobRepository.UpdateScheduleJobStopAll();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="jobkey"></param>
        public void StopJob(string jobkey)
        {
            QuartzHelper.PauseJob(jobkey);
            _scheduleJobRepository.UpdateScheduleJobStatus(jobkey,(int)JobStatus.Stop);
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="jobkey"></param>
        public void RunJob(string jobkey)
        {
            QuartzHelper.ResumeJob(jobkey);
            _scheduleJobRepository.UpdateScheduleJobStatus(jobkey, (int)JobStatus.Run);
        }

        public string UpdateJobCronExpression(string jobkey,string cronExpression)
        {
            var detail = _scheduleJobRepository.GetScheduleJobByJobKey(jobkey);
            if (detail == null)
            {
                return "无次数据";
            }
            var flag = QuartzHelper.ValidExpression(cronExpression);
            if (!flag)
            {
                return "cron 表达式错误";
            }
            detail.CronExpression = cronExpression;
            QuartzHelper.UpdateJob(detail);

            //_scheduleJobRepository.UpdateScheduleJobCronExpression(jobkey, cronExpression);

            return "";
        }

        public void ScheduleJob()
        {
            //var tt = new TaskModel()
            //{
            //    TaskID = new Guid(),
            //    Jobkey = "Jobkey2",
            //    TaskName = "Job2",
            //    CronExpressionString = "0/7 * * * * ? *",
            //    AssemblyName = "TaskManager.Service",
            //    ClassName = "HelloJob2",
            //    TypeString = "TaskManager.Service.HelloJob2, TaskManager.Service",
            //};
            //QuartzHelper.ScheduleJob(tt, true);
        }
    }
}
