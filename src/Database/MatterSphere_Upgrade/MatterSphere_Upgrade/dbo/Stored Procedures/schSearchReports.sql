

CREATE PROCEDURE [dbo].[schSearchReports] 
(
	@UI nvarchar(15) = '{default}'
	, @REPDESC nvarchar(50) = NULL
)

AS

SELECT
	SLC.schCode
	, CL.cdDesc AS [ReportDescription]
	, COALESCE(CL1.cdDesc, '~' + NULLIF(SLC.schGroup, '') + '~') AS [GroupDescription]
	, R.rptVersion
FROM
	dbSearchListConfig SLC
LEFT OUTER JOIN
	dbReport R ON R.rptCode = SLC.schCode
INNER JOIN
	dbCodeLookup CL ON CL.cdCode = SLC.schCode 
LEFT JOIN 
	dbo.GetCodeLookupDescription( 'SLGROUP', @UI ) CL1 ON CL1.cdCode =  SLC.schGroup

WHERE
	SLC.schIsReport = 1 AND
	CL.cdType = 'ADMINMENU' AND
	SLC.schSourceParameters NOT LIKE '%FWBS.OMS.%' AND
	CL.cdDesc LIKE '%' + @REPDESC + '%'
ORDER BY
	  CL.cdDesc

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchReports] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchReports] TO [OMSAdminRole]
    AS [dbo];

