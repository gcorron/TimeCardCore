CREATE procedure [dbo].[uJobDescr] @contractorId int, @jobId int, @descr varchar(50)
as
update Job set descr=@descr
where jobId=@jobId