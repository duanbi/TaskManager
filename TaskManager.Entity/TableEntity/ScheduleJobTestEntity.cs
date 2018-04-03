using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Entity.TableEntity
{
    public class ScheduleJobTestEntity
    {
        public int Id { get; set; }

        public string JobName { get; set; }

        public string JobKey { get; set; }

        public DateTime CreateTime { get; set; }

    }
}
