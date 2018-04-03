using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.Entity;
using TaskManager.Entity.DomainEntity;
using TaskManager.Service;

namespace TaskManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly JobManagerService _jobManagerService = new JobManagerService();
        private readonly ScheduleJobService _scheduleJobService = new ScheduleJobService();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetScheduleJobList()
        {
            var result = new List<ScheduleJobEntity>();
            result = _scheduleJobService.GetScheduleJobList();

            var gridModel = new DataSourceResult
            {
                Data = result.Select(x=>new ScheduleJobModel()
                {
                    Id = x.Id,
                    JobName = x.JobName,
                    JobKey = x.JobKey,
                    CronExpression = x.CronExpression,
                    JobStatus= x.JobStatus,
                    CreateTime = x.CreateTime,
                    CreateTimeString = x.CreateTime > DateTime.MinValue ? x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"): string.Empty,
                    LastExecuteTime = x.LastExecuteTime,
                    LastExecuteTimeString = x.LastExecuteTime > DateTime.MinValue ? x.LastExecuteTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,
                    NextExecuteTime = x.NextExecuteTime,
                    NextExecuteTimeString = x.NextExecuteTime > DateTime.MinValue ? x.NextExecuteTime.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty,

                }),
                Total = result.Count
            };

            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 全部暂停
        /// </summary>
        /// <returns></returns>
        public ActionResult StopAll()
        {
            _jobManagerService.StopAll();
            return Content("关闭执行完成");
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="jobkey"></param>
        /// <returns></returns>
        public ActionResult StopJob(string jobkey)
        {
            _jobManagerService.StopJob(jobkey);
            var result = new { status = true,message = ""};
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="jobkey"></param>
        /// <returns></returns>
        public ActionResult RunJob(string jobkey)
        {
            _jobManagerService.RunJob(jobkey);
            var result = new { status = true, message = "" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="jobkey"></param>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        public ActionResult UpdateJob(string jobkey, string cronExpression)
        {
            var result = new { status = true, message = "" };
            var response = _jobManagerService.UpdateJobCronExpression(jobkey, cronExpression);
            if (!string.IsNullOrWhiteSpace(response))
            {
                return Json(new { status = false, message = response }, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="jobkey"></param>
        /// <returns></returns>
        public ActionResult DeleteJob(string jobkey)
        {
            //TODO
            var result = new { status = true, message = "" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult AddJob()
        {
            _jobManagerService.ScheduleJob();
            return Content("修改执行完成");
        }

        /// <summary>
        /// CronExpression页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CronExpression()
        {
            return View();
        }

        /// <summary>
        /// 模拟执行时间
        /// </summary>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        public ActionResult CalcRunTime(string cronExpression)
        {
            var result = QuartzHelper.GetTaskeFireTime(cronExpression, 5);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UpdateCronExpression(ScheduleJobModel model)
        {
            var result = new { status = true, message = "" };
            var response = _jobManagerService.UpdateJobCronExpression(model.JobKey, model.CronExpression);
            if (!string.IsNullOrWhiteSpace(response))
            {
                return Json(new { status = false, message = response }, JsonRequestBehavior.AllowGet);
            }
            _scheduleJobService.UpdateScheduleJobCronExpressionAndName(model.JobKey, model.CronExpression,model.JobName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}