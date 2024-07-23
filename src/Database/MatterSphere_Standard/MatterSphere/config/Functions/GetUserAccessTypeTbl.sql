

CREATE FUNCTION [config].[GetUserAccessTypeTbl] ()
RETURNS @table table (AccessType ucodeLookup)

AS
BEGIN
INSERT INTO @table
SELECT AccessType from dbuser where usrADID = [config].[GetUserLogin] ()
RETURN
END

