

CREATE VIEW[dbo].[vwFolders]
AS
SELECT     flID AS ID, dbo.GetCodeLookupDesc(N'FOLDERS', flCode, '{DEFAULT}') AS FolderName, COALESCE(CL1.cdDesc, '~' + NULLIF(flType, '') + '~')
                      AS Type, N'' AS Item1, N'' AS Item2, N'' AS Item3
FROM         dbo.dbFolderStructure
LEFT JOIN 
dbo.GetCodeLookupDescription('FOLDERS', '{DEFAULT}') CL1 ON CL1.cdCode = flCode
WHERE     (ftIntType = N'FORMS')

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwFolders] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwFolders] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwFolders] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwFolders] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwFolders] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwFolders] TO [OMSApplicationRole]
    AS [dbo];

