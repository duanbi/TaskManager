using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Entity.TableEntity;

namespace TaskManager.Repository
{
    public class ScheduleJobTestRepository
    {
        public static readonly string SqlConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ToString();


        public void InsertScheduleJobTest(ScheduleJobTestEntity entity)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();
                var sql = @"INSERT  INTO dbo.ScheduleJobTest
                                    ( JobName, JobKey, CreateTime )
                            VALUES  ( @JobName, @JobKey, @CreateTime ) ";
                conn.Execute(sql, entity);

                conn.Close();
            }
        }
    }
}
