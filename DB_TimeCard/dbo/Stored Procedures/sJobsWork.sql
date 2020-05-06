CREATE procedure [dbo].[sJobsWork] @contractorId int, @startDay numeric(6,2)
as
-- exec sJobsWork 13,25
select jobId id, Descr
from vjob j
where (j.active=1
and j.jobId in (select jobid from jobstart where contractorId=@contractorId and startDay<=@startDay)
) or j.jobId in (select distinct jobid from work where contractorId=@contractorId and floor(@startDay)=floor(workDay) )
order by descr