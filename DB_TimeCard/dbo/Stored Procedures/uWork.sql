CREATE PROCEDURE [dbo].[uWork] @workId int, @contractorId int, @jobId int, @workDay decimal(6,2), @descr varchar(250), @hours decimal(4,2), @workType int
AS
if @WorkId = 0
BEGIN
	insert Work (contractorId, jobId, workDay, descr, [hours], workType)
		values(@contractorId, @jobId, @workDay, @descr, @hours, @workType)
END
else
BEGIN
	update Work
		set contractorId=@contractorId, jobId=@jobId, workDay=@workDay, descr=@descr,[hours]=@hours, workType=@workType
	where workId=@workId
END