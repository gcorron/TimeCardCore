create procedure [sPaymentsJob] @contractorId int, @jobId int
AS
select * from payment where contractorId = @contractorId and jobId=@jobId