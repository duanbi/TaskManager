using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Repository;

namespace TaskManager.Service
{
    public class BaseJob : IJob
    {
        private ScheduleJobTestRepository _scheduleJobTestRepository = new ScheduleJobTestRepository();
        public void Execute(IJobExecutionContext context)
        {
            //var detail = context.JobDetail;
            //_scheduleJobTestRepository.InsertScheduleJobTest(new Entity.TableEntity.ScheduleJobTestEntity()
            //{
            //    JobKey = detail.Key,
            //    JobName = detail.
            //});
        }
    }
}
