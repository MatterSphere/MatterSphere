

CREATE FUNCTION [config].[IsAdministratorTbl_NS] ( )
RETURNS @table table (IsAdmin bit)
AS
BEGIN
INSERT INTO @table
SELECT [config].[IsAdministrator_NS] ( ) 
RETURN
END
