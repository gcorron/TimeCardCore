CREATE procedure [dbo].[uJobStartClosed] @contractorId int, @jobId int, @closed bit
as
update jobstart set closed=@closed
where contractorId=@contractorId and jobId=@jobId