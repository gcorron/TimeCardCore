create procedure dPayment @payId int
as
delete payment where payId=@payId