CREATE procedure [dbo].[sJobTimeCardUnpaidCycles] @contractorId int, @jobId int
as
-- exec sJobTimeCardUnpaidCycles 13,62
select floor(workDay) workDay, sum(hours) hours
from work w
where jobId=@jobId
and not exists(
	select * from payment where contractorid=@contractorId and jobId=@jobId and workDay=floor(w.workDay))
group by floor(workDay)