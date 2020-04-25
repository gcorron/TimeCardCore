CREATE procedure [dbo].[sJobPaidThruDate] @contractorId int, @jobId int
as
/*
 exec sJobPaidThruDate 13,66
*/
declare @hours decimal(18,2)
set @hours=isnull((select sum(hours) from payment where contractorId=@contractorId and jobId=@jobId),0)
;with cte(workDay, hours, running) as
(
select workDay, hours,running=sum(hours) over(order by workDay, workId ROWS UNBOUNDED PRECEDING)
 from work w join JobStart js on w.contractorId=js.contractorId and w.jobId=js.jobId
 where w.contractorid=@contractorId and w.jobid=@jobId and w.workDay>=js.startDay
)
select top 1 workDay from cte where running<=@hours order by workDay desc