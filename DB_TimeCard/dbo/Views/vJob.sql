

CREATE view [dbo].[vJob]
as
select jobId, clientId, projectId, billType, j.active, concat(l1.val,' ',l2.val,' ',l3.val) shortDescr, concat(l1.descr,' ',l2.descr,' ',l3.descr) Descr
	,l1.descr client, l1.val clientCode, l2.descr project, l3.descr billtypeDescr
from job j join lookup l1 on j.clientId=l1.id
	join lookup l2 on j.projectId=l2.id
	join lookup l3 on j.billType=l3.id