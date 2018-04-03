
IF NOT EXISTS ( SELECT  1
                FROM    sysobjects
                WHERE   name = 'ScheduleJob'
                        AND type = 'u' )
    BEGIN	
        CREATE TABLE [dbo].[ScheduleJob]
            (
              [Id] INT PRIMARY KEY
                       IDENTITY(1, 1)
                       NOT NULL ,
              [JobName] NVARCHAR(200) NOT NULL ,
              [JobKey] NVARCHAR(200) NOT NULL ,
              [TypeString] NVARCHAR(200) NOT NULL ,
              [CronExpression] NVARCHAR(100) NOT NULL ,
              [CreateTime] DATETIME NOT NULL ,
              [JobStatus] INT NOT NULL
                              DEFAULT ( 0 ) ,
              [LastExecuteTime] DATETIME,
			  [NextExecuteTime] DATETIME,
			  [IsDelete] BIT NOT NULL,
			  [Remark] NVARCHAR(100)
            ); 
    END;
GO

IF NOT EXISTS ( SELECT  1
                FROM    sysobjects
                WHERE   name = 'ScheduleJobTest'
                        AND type = 'u' )
    BEGIN	
        CREATE TABLE [dbo].[ScheduleJobTest]
            (
              [Id] INT PRIMARY KEY
                       IDENTITY(1, 1)
                       NOT NULL ,
              [JobName] NVARCHAR(200) NOT NULL ,
              [JobKey] NVARCHAR(200) NOT NULL ,
              [CreateTime] DATETIME NOT NULL,
			); 
    END;
GO


IF NOT EXISTS ( SELECT  1
                FROM    [dbo].[ScheduleJob]
                WHERE   JobKey = 'Test_Schedule_job_1' )
    BEGIN

        INSERT  INTO dbo.ScheduleJob
                ( JobName ,
                  JobKey ,
                  TypeString ,
                  CronExpression ,
                  CreateTime ,
                  JobStatus ,
                  IsDelete 
		        )
        VALUES  ( N'Test_HelloJob_Job1' , -- JobName - nvarchar(200)
                  N'Test_Schedule_job_1' , -- JobKey - nvarchar(200)
                  N'TaskManager.Service.HelloJob, TaskManager.Service' , -- TypeString - nvarchar(200)
                  N'0/5 * * * * ? *' , -- CronExpressionString - nvarchar(100)
                  GETDATE() , -- CreateTime - datetime
                  1 ,  -- JobStatus - int
                  0
                );
    END;
GO


IF NOT EXISTS ( SELECT  1
                FROM    [dbo].[ScheduleJob]
                WHERE   JobKey = 'Test_Schedule_job_2' )
    BEGIN

        INSERT  INTO dbo.ScheduleJob
                ( JobName ,
                  JobKey ,
                  TypeString ,
                  CronExpression,
                  CreateTime ,
                  JobStatus ,
				  IsDelete
                )
        VALUES  ( N'Test_HelloJob2_Job2' , -- JobName - nvarchar(200)
                  N'Test_Schedule_job_2' , -- JobKey - nvarchar(200)
                  N'TaskManager.Service.HelloJob2, TaskManager.Service' , -- TypeString - nvarchar(200)
                  N'0/7 * * * * ? *' , -- CronExpressionString - nvarchar(100)
                  GETDATE() , -- CreateTime - datetime
                  1,  -- JobStatus - int
				  0
                );

    END;