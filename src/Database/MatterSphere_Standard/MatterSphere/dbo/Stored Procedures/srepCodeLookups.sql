

CREATE PROCEDURE [dbo].[srepCodeLookups]
(
	@UI uUICultureInfo='{default}',
	@TYPE nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(4000)

--- Select Statement for the base Query
SET @SQL = N'
SELECT
	CASE cdType
	WHEN ''SOURCE'' THEN ''Source of Introduction''
	WHEN ''SUBASSOC'' THEN ''Associated Formats''
	WHEN ''CONTTYPE'' THEN ''Contact Types''
	WHEN ''CLTYPE'' THEN ''Client Types''
	WHEN ''FILETYPE'' THEN ''File Types''
	WHEN ''DEPT'' THEN ''Departments''
	WHEN ''FUNDTYPE'' THEN ''Funding Types''
	WHEN ''IDENT'' THEN ''Proof of Identities''
	WHEN ''INFOADDTYPE'' THEN ''Address Types''
	WHEN ''INFOTYPE'' THEN ''Phone Types''
	WHEN ''LOCTYPE'' THEN ''Archive Document Locations''
	WHEN ''PRECCAT'' THEN ''Precedent Categories''
	WHEN ''PRECSUBCAT'' THEN ''Precedent Sub Categories''
	WHEN ''SUBCONTACT'' THEN ''Sub Contact Types''
	WHEN ''TASKTYPE'' THEN ''Task Types''
	WHEN ''USRROLES'' THEN ''User Roles''
	END AS clupDesc,
	CLUP.cdCode AS Code,
	CLUP.cdDesc AS Description,
	CLUP.cdHelp AS ToolTips
FROM         
	dbCodeLookup CLUP
WHERE
	CLUP.cdType = COALESCE(@TYPE, cdType) AND
	(cdType = ''SOURCE'' OR
	cdType = ''SUBASSOC'' OR
	cdType = ''CONTTYPE'' OR
	cdType = ''CLTYPE'' OR
	cdType = ''FILETYPE'' OR
	cdType = ''DEPT'' OR
	cdType = ''FUNDTYPE'' OR
	cdType = ''IDENT'' OR
	cdType = ''INFOADDTYPE'' OR
	cdType = ''INFOTYPE'' OR
	cdType = ''LOCTYPE'' OR
	cdType = ''PRECCAT'' OR
	cdType = ''PRECSUBCAT'' OR
	cdType = ''SUBCONTACT'' OR
	cdType = ''TASKTYPE'' OR
	cdType = ''USRROLES'')
ORDER BY
	CLUP.cdCode'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @TYPE nvarchar(50)', @UI, @TYPE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCodeLookups] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCodeLookups] TO [OMSAdminRole]
    AS [dbo];

