CREATE procedure [dbo].[spOrdersDataDML_Orders]
	@criteria VARCHAR(50) = '',
	@id int = null,
	@OrderDeliveryStatus int = 1,
	@OrderStatus int = 1,
	@Invoice INT = null,
	@OrderDate DATETIME2 = NULL,
	@ShippedDate DATETIME2 = NULL,
	@Company NVARCHAR(100) = null,
	@Store NVARCHAR(100) = null,
	@OrderTotal DECIMAL(18, 2) = 0.00,
	@PaymentTotal DECIMAL(18, 2) = 0.00,
	@RowsAffected SMALLINT OUTPUT

AS
BEGIN
	SET NOCOUNT ON;
	SET @RowsAffected = 0;

	IF @id IS NULL 
	BEGIN 
		RAISERROR('Id cannot be null', 16, 1);
		SET @RowsAffected = -1;
		RETURN;
	END

	BEGIN TRY
		IF (@criteria = 'Insert_or_update_order')
			BEGIN
				IF @id = 0
				BEGIN
					IF EXISTS( SELECT 1 FROM [dbo].[Orders] where Invoice = @Invoice)
					BEGIN
						SET @RowsAffected = -3;
						RETURN;
					END
					DECLARE @currentId INT;

					INSERT INTO [dbo].[Orders]
							   ([OrderDeliveryStatus]
							   ,[OrderStatus]
							   ,[Invoice]
							   ,[OrderDate]
							   ,[ShippedDate]
							   ,[Company]
							   ,[Store]
							   ,[OrderTotal]
							   ,[PaymentTotal])
						 VALUES
						 (@OrderDeliveryStatus, @OrderStatus, @Invoice, @OrderDate, @ShippedDate, @Company, @Store,
						 @OrderTotal, @PaymentTotal)
					SET @RowsAffected = @@ROWCOUNT;
				END
				ELSE
				BEGIN
					IF NOT EXISTS (SELECT 1 FROM [dbo].[Orders] WHERE id = @id)
						BEGIN
							SET @RowsAffected = -2; 
							RETURN;
						END;
					ELSE
						-- UPDATE RECORD
						BEGIN
							IF EXISTS( SELECT 1 FROM [dbo].[Orders] where Invoice = @Invoice and id != @id)
							BEGIN
								SET @RowsAffected = -3;
								RETURN;
							END
							UPDATE [dbo].[Orders] 
							SET
								OrderDeliveryStatus = @OrderDeliveryStatus,
							    OrderStatus = @OrderStatus,
							    Invoice = @Invoice,
							    OrderDate = @OrderDate,
							    ShippedDate = @ShippedDate,
							    Company = @Company,
							    Store = @Store,
							    OrderTotal = @OrderTotal,
							    PaymentTotal = @PaymentTotal
							WHERE id = @id;
							SET @RowsAffected = 1;
						END
				END
			END
		ELSE
			BEGIN
				IF(@criteria = 'Delete_record')
				BEGIN
					DELETE FROM [dbo].[Orders] where id = @id;					
				END
				SET @RowsAffected = 1;
				
			END
	END TRY
	BEGIN CATCH
		INSERT INTO [dbo].[ErrorLogs](ErrorMessage, ErrorLine, ProcedureName)
		VALUES(ERROR_MESSAGE(), ERROR_LINE(), 'spOrdersDataDML_Orders');
		SET @RowsAffected = -1;
	END CATCH
END
