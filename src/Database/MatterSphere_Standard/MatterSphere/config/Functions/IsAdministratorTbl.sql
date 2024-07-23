

CREATE FUNCTION [config].[IsAdministratorTbl] ( @USER nvarchar(200)  )
RETURNS @table table (IsAdmin bit)

AS
BEGIN
INSERT INTO @table
SELECT [config].[IsAdministrator] ( @USER ) 
RETURN
END
