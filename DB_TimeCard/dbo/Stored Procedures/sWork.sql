CREATE procedure [dbo].[sWork] @contractorId int, @workDay numeric(6,2), @payCycle bit
AS
-- exec sWork 0,0,1
select w.workId, w.contractorId, w.jobId, w.workDay, w.descr,w.hours,w.workType
	,l1.descr contractor
	,j.descr job
	,l2.descr workTypeDescr
from work w
	join lookup l1 on w.contractorId=l1.id
	join lookup l2 on w.workType=l2.id
	join vjob j on w.jobId=j.jobId
where contractorId=@contractorId
and (workday=@workDay or (floor(workday)=floor(@workDay) and @payCycle=1))
order by workDay,workid