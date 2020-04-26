create procedure uLookup @id int, @descr varchar(50), @val varchar(20), @active bit, @groupId int
AS
if @id=0
begin
	insert lookup (descr,val,active,groupId)
		values(@descr,@val, @active,@groupId)
end
else
begin
	update lookup set descr=@descr, val=@val, active=@active, groupId=@groupId
	where id=@id
end