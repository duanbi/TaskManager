using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Repository;

namespace TaskManager.Service
{
    public class HelloJob2 : IJob
    {
        private log4net.ILog log = log4net.LogManager.GetLogger("HelloJob2");
        private ScheduleJobTestRepository _scheduleJobTestRepository = new ScheduleJobTestRepository();
        public void Execute(IJobExecutionContext context)
        {
            log.Info(string.Format("HelloJob2"));
            _scheduleJobTestRepository.InsertScheduleJobTest(new Entity.TableEntity.ScheduleJobTestEntity()
            {
                JobKey = "Test_HelloJob2_Job2",
                JobName = "Test_HelloJob2_Job2",
                CreateTime = DateTime.Now
            });
        }
    }
}
