

CREATE PROCEDURE [dbo].[spGetOrders_Orders]
@id INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		BEGIN
			IF @id IS NOT NULL 
				BEGIN
					SELECT * FROM dbo.Orders WHERE id = @id;
					RETURN 0;
				END
			ELSE 
				BEGIN
					SELECT * FROM dbo.Orders;
					RETURN 0;
				END
		END
	END TRY
	BEGIN CATCH
		INSERT INTO [dbo].[ErrorLogs](ErrorMessage, ErrorLine, ProcedureName)
		VALUES(ERROR_MESSAGE(), ERROR_LINE(), 'spGetOrders_Orders');
		RETURN -1;
	END CATCH
END