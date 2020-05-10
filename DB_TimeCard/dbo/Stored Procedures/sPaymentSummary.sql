CREATE procedure [dbo].[sPaymentSummary] @contractorId int, @beforeCycle int
as
-- exec sPaymentSummary 13
;with hourSummary (jobId, totalHours)
as (
select w.jobId, sum(hours)
from work w join job j on w.jobid=j.jobid
left outer join jobStart js on j.jobId=js.jobId and js.contractorId=@contractorId
where w.contractorid=@contractorid and j.active=1
	and isnull(js.startDay,0)>0 and  w.workDay>=js.startDay and w.workDay < @beforeCycle
group by w.jobId
)

select j.jobid, js.startday, l1.descr client, l2.descr project, l3.descr billtype, isnull(h.totalHours,0) billed, isnull(sum(p.hours),0) paid
	,(select isnull(dbo.fJobPaidThruDate(@contractorId, j.jobId),0)) paidThruDay
from job j
	join lookup l1 on j.clientid=l1.id
	join lookup l2 on j.projectid=l2.id
	join lookup l3 on j.billtype=l3.id
	left outer join hourSummary h on j.jobId=h.jobId
	left outer join jobStart js on j.jobid=js.jobId and js.contractorId=@contractorId
	left outer join payment p on j.jobid=p.jobId and isnull(js.startDay,0)>0 and  p.workDay>=js.startDay
where j.active=1 and not (h.totalHours is null)

group by j.jobid, js.startDay, l1.descr, l2.descr, l3.descr,h.totalHours