create procedure sContractor @contractorId int
as
select * from Contractor
where contractorId=@contractorId