CREATE procedure [dbo].[uJobStart] @contractorId int, @jobId int, @startDay numeric(6,2), @isNew bit
as
-- exec uJobStart 13,66,22.06,0
declare @oldStartDay numeric(6,2) =null
if @isNew=0
begin
	select @oldStartDay=startDay from jobStart js join job j on js.jobId=j.jobId
	where contractorId=@contractorId and js.jobId=@jobId
		and j.billType<>(select id from lookup where groupId=(select groupid from LookupGroup where descr='BillType') and val='TC')
end

update jobStart set startDay=@startDay
where contractorId=@contractorId and jobId=@jobId

if @@ROWCOUNT=0
begin
	insert jobStart (contractorId, jobId, startDay)
		values (@contractorId, @jobId, @startDay)
end
else
begin
	if @oldStartDay is not null
	begin
		print concat('updating to ',@startDay,' from ',@oldStartDay)
		update payment set workDay=@startDay
		where contractorId=@contractorId and jobId=@jobId and workDay=@oldStartDay
	end
end