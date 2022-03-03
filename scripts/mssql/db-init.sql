CREATE DATABASE TestDB;
SELECT Name from sys.Databases;
GO
USE TestDB;
GO
CREATE TABLE Inventory (CMDB_ID INT, Name NVARCHAR(50), Qty INT, Is_Stack_Member BIT);
GO
INSERT INTO Inventory VALUES (1234, 'Bwaaaa', 150, 1);
INSERT INTO Inventory VALUES (5678, 'Huuuww', 201, 0);
GO
