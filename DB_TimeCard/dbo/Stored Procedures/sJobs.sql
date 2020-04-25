CREATE procedure [dbo].[sJobs]
as
select jobId id, Descr
from vjob
order by descr