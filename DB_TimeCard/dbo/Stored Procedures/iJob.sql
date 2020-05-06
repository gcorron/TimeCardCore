create procedure iJob @clientId int, @projectId int, @billtype int
AS
	if exists(select * from job where clientId=@clientId and projectId=@projectId and billtype=@billType)
	begin
		raiserror('Cannot create duplicate job.',11,1)
		return
	end
	insert job (clientId, projectId, billType,active)
		values(@clientId, @projectId, @billType, 1)