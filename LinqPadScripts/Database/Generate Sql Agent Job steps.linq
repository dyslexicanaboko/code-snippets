<Query Kind="Program" />

//Generate a SQL Agent job that has a step per database. The steps will run regardless of failure.
void Main()
{
	var lst = GetRestoreTargets("0.0.0.0",
		"DatabaseA",
		"DatabaseB",
		"DatabaseC");

	lst.Dump();
	
	GetSteps(lst);
}

public void GetSteps(List<RestoreTarget> targets)
{
	var step = 1;
	var maxSteps = targets.Count;
	var sb = new StringBuilder();
	
	foreach (var t in targets)
	{
		var txtStep = GetStep(t, step, maxSteps);
		
		sb.AppendLine(txtStep);
		
		step++;
	}
	
	var txtSteps = sb.ToString();
	
	GetJobBody(txtSteps).Dump();
}

public List<RestoreTarget> GetRestoreTargets(string destinationServer, params string[] databaseNames)
{
	return new List<RestoreTarget>();
}

private string GetStep(RestoreTarget target, int step, int maxSteps)
{
	var t = target;
	
	var cmd = $"CMD command here";

	var nextStep = 0;
	var actionSuccess = 1;
	var actionFail = 2;
	
	if (step != maxSteps)
	{
		nextStep = step + 1;
		actionSuccess = 3;
		actionFail = 3;
	}

	//Action 1 = stop and report success
	//Action 2 = stop and report failure
	//Action 3 = go to next following step
	//Action 4 = go to next specific step
	var sql = $@"
	EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id = @jobId, @step_name = N'{t.DatabaseName}', 
		@step_id = {step}, 
		@cmdexec_success_code = 0, 
		@on_success_action = {actionSuccess}, 
		@on_success_step_id = {nextStep}, 
		@on_fail_action = {actionFail}, 
		@on_fail_step_id = {nextStep}, 
		@retry_attempts = 0, 
		@retry_interval = 0, 
		@os_run_priority = 0, @subsystem = N'CmdExec', 
		@command = N'{cmd}', 
		@output_file_name = N'C:\Dump\Log-{t.DatabaseName}.log', 
		@flags = 32, 
		@proxy_name = N'Power Shell SQL SVC Account'
IF(@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
	";
		
	return sql;
}

public class IgnoreCaseCompare : IEqualityComparer<string>
{
	public bool Equals(string x, string y)
	{
		return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
	}

	public int GetHashCode(string obj)
	{
		return obj.GetHashCode();
	}
}

public class RestoreTarget
{
	public string SourceServer { get; set; }
	
	public string DestinationServer { get; set; }
	
	public string DatabaseName { get; set; }
}

private string GetJobBody(string steps)
{
	var str = $@"USE [msdb]
GO

/****** Object:  Job [DevOps - Post prod deploy refresh]    Script Date: 12/17/2020 22:24:32 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 12/17/2020 22:24:32 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'DevOps - Post prod deploy refresh', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

{steps}

EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO";

	return str;
}