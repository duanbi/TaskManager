using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Repository;

namespace TaskManager.Service
{
    public class HelloJob : IJob
    {
        private log4net.ILog log = log4net.LogManager.GetLogger("HelloJob");
        private ScheduleJobTestRepository _scheduleJobTestRepository = new ScheduleJobTestRepository();
        public void Execute(IJobExecutionContext context)
        {
            _scheduleJobTestRepository.InsertScheduleJobTest(new Entity.TableEntity.ScheduleJobTestEntity()
            {
                JobKey = "Test_HelloJob2_Job1",
                JobName = "Test_HelloJob2_Job1",
                CreateTime = DateTime.Now
            });
        }
    }
}
