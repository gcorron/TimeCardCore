CREATE procedure [dbo].[sJobStart] @contractorId int
as
select j.jobid, descr, js.startday, client, project, billTypeDescr
	from vjob j left outer join jobstart js on j.jobId=js.jobid and js.contractorId=@contractorid
where j.active=1
order by client, project, billtypedescr