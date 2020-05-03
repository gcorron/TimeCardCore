create procedure dAppRole @roleId int
AS
delete AppRole
where roleId=@roleId