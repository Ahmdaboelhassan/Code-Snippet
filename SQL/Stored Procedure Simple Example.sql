CREATE PROCEDURE GetEmployeesByDepartment
    @Department NVARCHAR(50)
AS
BEGIN
    -- Selecting employees based on the department
    SELECT EmployeeID, FirstName, LastName, Department, HireDate
    FROM Employees
    WHERE Department = @Department;
END;

-- Usage
EXEC GetEmployeesByDepartment @Department = 'Sales';