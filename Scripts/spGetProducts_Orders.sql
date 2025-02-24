
CREATE PROCEDURE [dbo].[spGetProducts_Orders]
@orderId INT
AS
BEGIN TRY
	select p.ProductId, p.ProductName, p.UnitPrice, p.NoOfUnits, p.Discount, p.OrderId from Orders o join Products P ON o.id = p.OrderId
	where o.id = @orderId
END TRY
BEGIN CATCH
	INSERT INTO [dbo].[ErrorLogs](ErrorMessage, ErrorLine, ProcedureName)
	VALUES(ERROR_MESSAGE(), ERROR_LINE(), 'spGetProducts_Orders');
END CATCH