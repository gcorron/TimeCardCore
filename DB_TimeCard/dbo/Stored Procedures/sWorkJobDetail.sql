create procedure sWorkJobDetail @contractorId int, @jobId int
AS
declare @startDay numeric(6,2)

select @startDay=startDay from jobStart where jobId=@jobId and contractorId=@contractorId

select w.workId, w.contractorId, w.jobId, w.workDay, w.descr,w.hours,w.workType
	,l1.descr contractor
	,j.descr job
	,l2.descr workTypeDescr
from work w
	join lookup l1 on w.contractorId=l1.id
	join lookup l2 on w.workType=l2.id
	join vjob j on w.jobId=j.jobId
where w.contractorId=@contractorId and w.jobId=@jobId
	and (@startDay is null or workDay>=@startDay)
order by workDay,workid