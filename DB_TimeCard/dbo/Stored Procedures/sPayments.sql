create procedure sPayments @contractorId int
AS
select p.jobid, payDate,checkNo, hours, workDay
from payment p join jobStart js on p.jobId=js.jobId and js.contractorId=p.contractorId
where p.contractorid=@contractorId and p.workday >= js.startDay