create procedure sWorkOpen @contractorId int
as
select workCycle from WorkOpen
where contractorId=@contractorId