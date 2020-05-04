CREATE procedure [dbo].[sWorkExtended] @contractorId int, @workDay numeric(6,2), @payCycle bit
AS
-- exec sWorkExtended 13,33,0

select w.workId, w.contractorId, w.jobId, w.workDay, w.descr,w.hours
	,l1.descr contractor
	,j.descr job
	,j.clientId, j.projectId
	,j.client, j.clientCode, j.project, l2.descr billType, l5.descr workType
from work w
	join lookup l1 on w.contractorId=l1.id
	join vjob j on w.jobId=j.jobId
	join jobStart js on w.contractorId = js.contractorId and w.jobId=js.jobId
	join lookup l2 on j.billType=l2.id
	join lookup l5 on w.workType=l5.id
where w.contractorId=@contractorId
and (
	(@payCycle = 0 and (floor(workday) between js.startDay and @workDay))
	or (@payCycle = 1 and (floor(workday) = floor(@workDay)))
	)	
order by workDay,workid