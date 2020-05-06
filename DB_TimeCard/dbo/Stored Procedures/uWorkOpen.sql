create procedure uWorkOpen @contractorId int, @workCycle int
as
delete WorkOpen
where contractorId=@contractorId and WorkCycle=@workCycle

if @@ROWCOUNT=0
begin
	insert WorkOpen (contractorId, workCycle) values(@contractorId, @workCycle)
end