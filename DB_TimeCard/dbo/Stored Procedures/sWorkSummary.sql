CREATE procedure [dbo].[sWorkSummary] @contractorId int
AS

-- exec sWorkSummary 13
select w.jobId, j.descr,  floor(w.workDay) workPeriod, sum(hours) hours
from work w join vjob j on w.jobid=j.jobid
left outer join jobStart js on j.jobId=js.jobId and js.contractorId=@contractorId
where w.contractorid=@contractorid and j.active=1
	and isnull(js.startDay,0)>0 and  w.workDay>=js.startDay
group by w.jobId, j.descr, floor(w.workDay)