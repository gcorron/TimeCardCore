create function fJobPaidThruDate(@contractorId int, @jobId int) returns decimal(6,2)
AS
BEGIN
	declare @hours decimal(18,2)
	declare @workDay decimal(6,2)
	set @hours=isnull((select sum(hours) from payment where contractorId=@contractorId and jobId=@jobId),0)
	;with cte(workDay, hours, running) as
	(
	select workDay, hours,running=sum(hours) over(order by workDay, workId ROWS UNBOUNDED PRECEDING)
	 from work w join JobStart js on w.contractorId=js.contractorId and w.jobId=js.jobId
	 where w.contractorid=@contractorId and w.jobid=@jobId and w.workDay>=js.startDay
	)
	select top 1 @workDay=workDay from cte where running<=@hours order by workDay desc
	return @workDay

END