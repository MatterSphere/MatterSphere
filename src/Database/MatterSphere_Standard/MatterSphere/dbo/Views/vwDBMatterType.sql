

CREATE VIEW[dbo].[vwDBMatterType]
AS
SELECT     typeCode AS MatterType, LEFT(COALESCE(CL1.cdDesc, '~' + NULLIF(typeCode, '') + '~'), 50) AS MatTypeDesc, fileDeptCode AS MatDepartment, 
                      fileAccCode AS MatAccNum, COALESCE (fileDefBankCode, N'1') AS MatDefBankCode, COALESCE (fileDefOffBankCode, N'1') 
                      AS MatDefOffBankCode
FROM         dbo.dbFileType
LEFT JOIN 
dbo.GetCodeLookupDescription('FILETYPE', 'en-gb') CL1 ON CL1.cdCode = typeCode
GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBMatterType] TO [OMSApplicationRole]
    AS [dbo];

