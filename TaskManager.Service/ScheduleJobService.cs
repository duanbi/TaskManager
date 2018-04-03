using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entity;
using TaskManager.Repository;

namespace TaskManager.Service
{
    public class ScheduleJobService
    {
        private ScheduleJobRepository _scheduleJobRepository = new ScheduleJobRepository();

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<ScheduleJobEntity> GetScheduleJobList()
        {
            var result = _scheduleJobRepository.GetScheduleJobList();
            return result;
        }

        /// <summary>
        /// 通过ID获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ScheduleJobEntity GetScheduleJobById(int id)
        {
            var result = _scheduleJobRepository.GetScheduleJobById(id);
            return result;

            
        }

        public void UpdateScheduleJobCronExpressionAndName(string jobKey, string cronExpression, string jobName)
        {
            _scheduleJobRepository.UpdateScheduleJobCronExpressionAndName(jobKey, cronExpression, jobName);
        }

    }
}
