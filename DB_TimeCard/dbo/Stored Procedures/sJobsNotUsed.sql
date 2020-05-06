CREATE procedure [dbo].[sJobsNotUsed]
as
-- exec sJobsNotUsed
select jobId id, Descr
from vjob j
where j.jobId not in (select distinct jobid from jobstart)
 and j.jobId not in (select distinct jobid from work)
order by descr