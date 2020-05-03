create procedure uAppRole @id int, @name varchar(50)
AS

if @Id = 0
begin
	insert AppRole (roleName)
		values(@name)
	select SCOPE_IDENTITY()
end
else
begin
	update AppRole
		set roleName=@name
	where roleId=@id
	select @id
end