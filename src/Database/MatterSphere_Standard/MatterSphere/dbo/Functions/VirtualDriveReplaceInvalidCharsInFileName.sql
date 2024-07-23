CREATE FUNCTION [dbo].[VirtualDriveReplaceInvalidCharsInFileName] (@FileName NVARCHAR(255))
RETURNS NVARCHAR(255)
AS
BEGIN
	RETURN REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@FileName, ':','_'), '/','_'), '\','_'), '*','_'), '|','_'), '?','_'), '<','_'), '>','_'), '"',''''), CHAR(13),' '), CHAR(10),' '), CHAR(9),' ')
END

GO
GRANT EXECUTE
	ON OBJECT::[dbo].[VirtualDriveReplaceInvalidCharsInFileName] TO [OMSRole]
	AS [dbo];

GO
GRANT EXECUTE
	ON OBJECT::[dbo].[VirtualDriveReplaceInvalidCharsInFileName] TO [OMSAdminRole]
	AS [dbo];
