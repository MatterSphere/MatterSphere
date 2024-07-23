

-- =============================================
-- Author:		Renato Nappo
-- Create date: 31.05.16
-- Description:	Return the version header data for the specified version ID from dbVersionDataHeader
-- =============================================
CREATE PROCEDURE [dbo].[sprObjectLinkedItemData] 
(
	@versionID nvarchar(50) = null
)
AS
BEGIN
	SET NOCOUNT ON;

SELECT  case n.c.value('(Table)[1]','varchar(128)')
			when 'dbSearchListVersionData' then 'Search List'
			when 'dbEnquiryVersionData' then 'Enquiry Form'
			when 'dbScriptVersionData' then 'Script'
			when 'dbDataListVersionData' then 'Data List'
			when 'dbFileManagementVersionData' then 'File Management'
		end as 'ObjectType',
		n.c.value('(Code)[1]','varchar(128)') AS 'Code',  
        n.c.value('(Version)[1]','varchar(128)') AS 'VersionNumber',
		n.c.value('(Table)[1]','varchar(128)') AS 'TableName'
FROM    dbVersionDataHeader t
Cross   Apply versionLinks.nodes('/VersionData/LinkedItems/LinkItem') n(c)  
WHERE versionID = @versionID

END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprObjectLinkedItemData] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprObjectLinkedItemData] TO [OMSAdminRole]
    AS [dbo];

