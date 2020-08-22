CREATE procedure [dbo].[sWorkSummary] @contractorId int, @formal_descr bit=0
AS

-- exec sWorkSummary 13
select w.jobId, case @formal_descr when 0 then j.descr else concat(j.client,' ',j.project,' ',j.billtypeDescr) end descr ,  floor(w.workDay) workPeriod, sum(hours) hours
from work w join vjob j on w.jobid=j.jobid
left outer join jobStart js on j.jobId=js.jobId and js.contractorId=@contractorId
where w.contractorid=@contractorid and j.active=1
	and isnull(js.startDay,0)>0 and  w.workDay>=js.startDay
group by w.jobId, j.descr,j.client, j.project,j.billtypeDescr, floor(w.workDay)