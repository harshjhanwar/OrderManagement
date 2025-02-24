CREATE procedure [dbo].[spProductsDataDML_Orders]
	@criteria VARCHAR(50) = '',
	@id int = null,
	@ProductName  varchar(100) = NULL,
	@UnitPrice DECIMAL(18, 2) = 0.00,
	@NoOfUnits INT = null,
	@Discount DECIMAL(18, 2) = 0.00,
	@OrderId INT = NULL,
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
		IF (@criteria = 'Insert_or_update_product')
			BEGIN
				IF @id = 0
				BEGIN
					IF @OrderId IS NULL
					BEGIN 
						RAISERROR('OrderId cannot be null', 16, 1);
						SET @RowsAffected = -1;
						RETURN;
					END
					DECLARE @currentId INT;

					INSERT INTO Products(ProductName, UnitPrice, NoOfUnits, Discount, OrderId)
					VALUES(@ProductName, @UnitPrice, @NoOfUnits, @Discount, @OrderId)
					SET @RowsAffected = @@ROWCOUNT;
				END
				ELSE
				BEGIN
					IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductId = @id)
						BEGIN
							SET @RowsAffected = -2; 
							RETURN;
						END;
					ELSE
						BEGIN
							UPDATE [dbo].[Products] 
							SET
								ProductName = @ProductName,
								UnitPrice = @UnitPrice,
								NoOfUnits = @NoOfUnits,
								Discount = @Discount
							WHERE ProductId = @id;
							SET @RowsAffected = 1;
						END
				END
			END
		ELSE
			BEGIN
				IF(@criteria = 'Delete_product')
				BEGIN
					DELETE FROM [dbo].[Products] where ProductId = @id;
					SET @RowsAffected = 1;
				END
				
			END
	END TRY
	BEGIN CATCH
		INSERT INTO [dbo].[ErrorLogs](ErrorMessage, ErrorLine, ProcedureName)
		VALUES(ERROR_MESSAGE(), ERROR_LINE(), 'spProductsDataDML_Orders');
		SET @RowsAffected = -1;
	END CATCH
END