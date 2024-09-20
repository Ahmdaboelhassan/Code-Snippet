-- Scalar Function
CREATE FUNCTION dbo.SquareNumber(@Number INT)
RETURNS INT
AS
BEGIN
    RETURN @Number * @Number;
END;

-- usage
SELECT dbo.SquareNumber(5) AS SquareOfFive;  -- Returns 25




-- Inline Table-Valued Function
CREATE FUNCTION dbo.GetProductsAbovePrice(@MinPrice DECIMAL(10, 2))
RETURNS TABLE
AS
RETURN
(
    SELECT ProductID, ProductName, Price
    FROM Products
    WHERE Price > @MinPrice
);

-- usage
SELECT * FROM dbo.GetProductsAbovePrice(50.00);




-- Multi-Statement Table-Valued Function
CREATE FUNCTION dbo.GetLowStockProducts(@Threshold INT)
RETURNS @LowStockProducts TABLE
(
    ProductID INT,
    ProductName NVARCHAR(100),
    StockQuantity INT
)
AS
BEGIN
    INSERT INTO @LowStockProducts
    SELECT ProductID, ProductName, StockQuantity
    FROM Products
    WHERE StockQuantity < @Threshold;

    RETURN;
END;

-- usage
SELECT * FROM dbo.GetLowStockProducts(10);
