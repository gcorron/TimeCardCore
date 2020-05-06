CREATE procedure dJob @jobId int
as
if exists(select jobId from vjob j
	where j.jobId not in (select distinct jobid from jobstart)
	and j.jobId not in (select distinct jobid from work)
 )
 begin
	delete job where jobId=@jobId
 end
 else
 begin
	raiserror('Cannot delete job because it is in use.',11,1)
 end