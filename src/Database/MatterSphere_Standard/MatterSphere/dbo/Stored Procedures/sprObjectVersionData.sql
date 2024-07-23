

-- =============================================
-- Author:		Renato Nappo
-- Create date: 27.05.16
-- Description:	Return the version data for the specified object
-- =============================================
CREATE PROCEDURE [dbo].[sprObjectVersionData] 
(
	@code nvarchar(15) = null,
	@version nvarchar(10) = null,
	@tablename nvarchar(30) = null
)
AS
BEGIN
	SET NOCOUNT ON;

SELECT
	*
FROM
	dbVersionDataHeader
WHERE
	CONVERT(nvarchar(max), VersionLinks) like '%<Code>' + @code + '</Code><Version>' + @version + '</Version><Table>' + @tablename + '</Table>%'
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprObjectVersionData] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprObjectVersionData] TO [OMSAdminRole]
    AS [dbo];

