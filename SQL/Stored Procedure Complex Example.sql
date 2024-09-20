-- Create Fuction To Use It In stored Procedure
CREATE FUNCTION dbo.CalculateDiscount(@Price DECIMAL(10, 2), @DiscountRate DECIMAL(5, 2))
RETURNS DECIMAL(10, 2)
AS
BEGIN
    RETURN @Price * (1 - @DiscountRate / 100);
END;

-- Create The Stored Procedure
CREATE PROCEDURE UpdateProductInfo
    @ProductID INT,
    @NewPrice DECIMAL(10, 2),
    @StockChange INT,      -- INOUT Parameter
    @DiscountRate DECIMAL(5, 2),
    @FinalPrice DECIMAL(10, 2) OUT
AS
BEGIN
    DECLARE @CurrentStock INT;

    -- Check if the product exists
    IF EXISTS (SELECT 1 FROM Products WHERE ProductID = @ProductID)
    BEGIN
        -- Update the product price
        UPDATE Products
        SET Price = @NewPrice
        WHERE ProductID = @ProductID;

        -- Adjust the stock quantity
        SELECT @CurrentStock = StockQuantity FROM Products WHERE ProductID = @ProductID;
        SET @CurrentStock = @CurrentStock + @StockChange;
        UPDATE Products
        SET StockQuantity = @CurrentStock
        WHERE ProductID = @ProductID;

        -- Calculate the final price after applying the discount
        SET @FinalPrice = dbo.CalculateDiscount(@NewPrice, @DiscountRate);
    END
    ELSE
    BEGIN
        -- If product does not exist, set default values
        SET @FinalPrice = 0;
        SET @StockChange = 0;
    END;
END;


--------------------------------------------------------------------
-- Usage
DECLARE @UpdatedPrice DECIMAL(10, 2);
DECLARE @AdjustedStock INT;

-- Initial values
SET @AdjustedStock = 5;

EXEC UpdateProductInfo
    @ProductID = 1,
    @NewPrice = 99.99,
    @StockChange = @AdjustedStock OUTPUT,  -- INOUT parameter
    @DiscountRate = 10,
    @FinalPrice = @UpdatedPrice OUTPUT;    -- OUT parameter

-- View the results
SELECT @UpdatedPrice AS FinalPrice, @AdjustedStock AS AdjustedStock;