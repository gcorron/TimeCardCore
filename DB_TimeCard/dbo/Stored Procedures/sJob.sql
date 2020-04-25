

CREATE procedure [dbo].[sJob] @jobId int
as
-- exec sJobIsTimeCard 34
select lu1.descr client, lu2.descr project, lu3.descr billtypeDescr, j.descr
from job j
join lookup lu1 on j.clientId=lu1.id
join lookup lu2 on j.projectId=lu2.id
join lookup lu3 on j.billType=lu3.id
where jobid=@jobid