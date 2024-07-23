

CREATE PROCEDURE [dbo].[srepFilKeyDatesList]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(50) = null,
	@FILENO nvarchar(50) = null
)

AS 

DECLARE @SQL nvarchar(4000)

--- Select Statement for the base Query
SET @SQL = N'
SELECT     
	CL.clNo, 
	F.fileNo, 
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	KD.kdDesc, 
	KD.kdDate, 
	U.usrInits, 
        F.fileStatus, 
	U.usrFullName
FROM         
	dbo.dbFile F
INNER JOIN
	dbo.dbClient CL ON F.clID = CL.clID 
INNER JOIN
	dbo.dbKeyDates KD ON F.fileID = KD.fileID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
WHERE
	KD.kdActive = 1 AND
	CL.clNo = COALESCE(@CLNO, CL.clNo) AND
	F.fileNo = COALESCE(@FILENO, F.fileNo)'

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @CLNO nvarchar(50), @FILENO nvarchar(50)', @UI, @CLNO, @FILENO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilKeyDatesList] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilKeyDatesList] TO [OMSAdminRole]
    AS [dbo];

