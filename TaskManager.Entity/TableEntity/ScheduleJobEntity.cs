using System;

namespace TaskManager.Entity
{
    public class ScheduleJobEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 任务唯一Key
        /// </summary>
        public string JobKey { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string TypeString { get; set; }

        /// <summary>
        /// Cron值
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public int JobStatus{ get; set; }

        /// <summary>
        /// 最后一次执行时间
        /// </summary>
        public DateTime LastExecuteTime { get; set; }

        /// <summary>
        /// 下一次执行时间
        /// </summary>
        public DateTime NextExecuteTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public enum JobStatus
    {
        Stop = 0,

        Run = 1
    }
}
