using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TaskManager.Entity;

namespace TaskManager.Repository
{
    public class ScheduleJobRepository
    {

        public static readonly string SqlConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ToString();

        public List<ScheduleJobEntity> GetScheduleJobList()
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();

                var sql = @"SELECT  [Id] ,
                                    [JobName] ,
                                    [JobKey] ,
                                    [TypeString] ,
                                    [CronExpression] ,
                                    [CreateTime] ,
                                    [JobStatus] ,
                                    [LastExecuteTime] ,
                                    [NextExecuteTime] ,
                                    [IsDelete] ,
                                    [Remark]
                            FROM    [dbo].[ScheduleJob] WITH ( NOLOCK )
                            WHERE   IsDelete = 0 ";
                var result = conn.Query<ScheduleJobEntity>(sql);
                conn.Close();
                return result.ToList();
            }
        }

        public ScheduleJobEntity GetScheduleJobByJobKey(string jobKey)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();

                var sql = @"SELECT  [Id] ,
                                    [JobName] ,
                                    [JobKey] ,
                                    [TypeString] ,
                                    [CronExpression] ,
                                    [CreateTime] ,
                                    [JobStatus] ,
                                    [LastExecuteTime] ,
                                    [NextExecuteTime] ,
                                    [IsDelete] ,
                                    [Remark]
                            FROM    [dbo].[ScheduleJob] WITH ( NOLOCK )
                            WHERE   IsDelete = 0
                                    AND JobKey = @JobKey ";
                var result = conn.QueryFirstOrDefault<ScheduleJobEntity>(sql,new { JobKey  = jobKey });
                conn.Close();
                return result;
            }
        }

        public ScheduleJobEntity GetScheduleJobById(int Id)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();

                var sql = @"SELECT  [Id] ,
                                    [JobName] ,
                                    [JobKey] ,
                                    [TypeString] ,
                                    [CronExpression] ,
                                    [CreateTime] ,
                                    [JobStatus] ,
                                    [LastExecuteTime] ,
                                    [NextExecuteTime] ,
                                    [IsDelete] ,
                                    [Remark]
                            FROM    [dbo].[ScheduleJob] WITH ( NOLOCK )
                            WHERE   IsDelete = 0
                                    AND Id = @Id ";
                var result = conn.QueryFirstOrDefault<ScheduleJobEntity>(sql, new { Id = Id });
                conn.Close();
                return result;
            }
        }

        public void UpdateScheduleJob(ScheduleJobEntity entity)
        {

        }


        public void UpdateScheduleJobStopAll()
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();
                conn.Execute(" UPDATE dbo.ScheduleJob SET JobStatus = 0 ");
                conn.Close();
            }
        }


        public void UpdateScheduleJobStatus(string jobKey,int status)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();
                conn.Execute(" UPDATE dbo.ScheduleJob SET JobStatus = @JobStatus WHERE JobKey = @JobKey ", new { JobStatus = status, JobKey = jobKey });
                conn.Close();
            }
        }

        public void UpdateScheduleJobCronExpression(string jobKey, string cronExpression)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();
                conn.Execute(" UPDATE dbo.ScheduleJob SET CronExpression = @CronExpressionString WHERE JobKey = @JobKey ", new { CronExpressionString = cronExpression, JobKey = jobKey });
                conn.Close();
            }
        }

        public void UpdateScheduleJobCronExpressionAndName(string jobKey, string cronExpression,string jobName)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();
                conn.Execute(" UPDATE dbo.ScheduleJob SET CronExpression = @CronExpressionString, JobName = @JobName WHERE JobKey = @JobKey ", new { CronExpressionString = cronExpression, JobKey = jobKey, JobName = jobName });
                conn.Close();
            }
        }



        public void UpdateLastExecuteUtc(string jobKey, DateTime executeUtc)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();
                conn.Execute(" UPDATE dbo.ScheduleJob SET LastExecuteUtc = @LastExecuteUtc WHERE JobKey = @JobKey ", new { LastExecuteUtc = executeUtc, JobKey = jobKey });
                conn.Close();
            }
        }

        public void UpdateNextExecuteUtc(string jobKey, DateTime executeUtc)
        {
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnectionString))
            {
                conn.Open();
                conn.Execute(" UPDATE dbo.ScheduleJob SET NextExecuteUtc = @NextExecuteUtc WHERE JobKey = @JobKey ", new { NextExecuteUtc = executeUtc, JobKey = jobKey });
                conn.Close();
            }
        }

    }
}
