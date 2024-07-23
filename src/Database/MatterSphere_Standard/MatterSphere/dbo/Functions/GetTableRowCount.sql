CREATE FUNCTION [dbo].[GetTableRowCount] (@TableName sysname)
RETURNS INT
AS
BEGIN
	RETURN (SELECT TOP 1 rows FROM sysindexes WHERE id = OBJECT_ID(@TableName) AND indid < 2)
END

GO
