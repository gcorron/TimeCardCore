CREATE procedure [dbo].[uContractor] @contractorId int, @invoiceName varchar(50), @invoiceAddress varchar(250), @rate decimal(18,2)
as
update Contractor
	set InvoiceName=@invoiceName, InvoiceAddress=@invoiceAddress, Rate=@rate
where contractorId=@contractorId

if @@ROWCOUNT = 0
begin
	insert Contractor (ContractorId, InvoiceName, InvoiceAddress, Rate)
		values(@contractorId, @invoiceName, @invoiceAddress, @rate)
end