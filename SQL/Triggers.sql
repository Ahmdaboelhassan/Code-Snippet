CREATE TRIGGER trgAfterInsertEmployee
ON Employees
AFTER INSERT
AS
BEGIN
    INSERT INTO AuditLog (EmployeeID, FirstName, LastName, Department, ActionType)
    SELECT EmployeeID, FirstName, LastName, Department, 'INSERT'
    FROM inserted;
END;
