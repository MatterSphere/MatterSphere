

CREATE PROCEDURE [dbo].[srepConAssocs]
(
	@CONTNAME nvarchar(128) = null,
	@CONTTYPE nvarchar(15) = null,
	@FILESTATUS nvarchar(15) = null,
	@INTERACTIVE bit = null,
	@SUBCONTTYPE nvarchar(15) = null,
	@UI uUICultureInfo='{default}'
)

AS 

DECLARE @SQL nvarchar(4000)
DECLARE @SELECT nvarchar(2500)
DECLARE @WHERE nvarchar(1500)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT     
	C.contName, 
	A.addLine1, 
	A.addLine2, 
	A.addLine3, 
	A.addLine4, 
    A.addLine5, 
	A.addPostcode, 
	CL.clNo, 
	F.fileNo, 
	X.cdDesc AS contTypeCode,
	replace(F.fileDesc, char(13) + char(10), N'''') as fileDesc, 
	U.usrInits, 
	CL.clName,
	Y.cdDesc AS CountryDesc
FROM         
	dbo.dbContact C
INNER JOIN
	dbo.dbAssociates ASS ON ASS.contID = C.contID
INNER JOIN
	dbo.dbFile F ON F.fileID = ASS.fileID
INNER JOIN
	dbo.dbAddress A ON C.contDefaultAddress = A.addID 
INNER JOIN
	dbo.dbClient CL ON CL.clID = F.clID
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID 
LEFT OUTER JOIN
	dbo.dbCountry CTRY ON A.addCountry = CTRY.ctryID
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''CONTTYPE'', @UI) AS X ON X.cdCode = C.contTypeCode
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''COUNTRIES'', @UI) AS Y ON Y.cdCode = CTRY.ctryCode '

--- Build Where Clause
SET @WHERE = ''

--- @CONTTYPE clause
IF(@CONTTYPE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' C.ContTypeCode = @CONTTYPE '
END

--- @SUBCONTTYPE clause
IF(@SUBCONTTYPE IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @WHERE = @WHERE + ' C.ContAddFilter = @SUBCONTTYPE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND C.ContAddFilter = @SUBCONTTYPE '
	END
END

--- @CONTNAME clause, this is a free text field so an extra check is required
IF(@CONTNAME IS NOT NULL AND @CONTNAME <> '')
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @WHERE = @WHERE + ' C.ContName LIKE + ''%'' + @CONTNAME + ''%'' '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND C.ContName LIKE + ''%'' + @CONTNAME + ''%'' '
	END
END

--- @FILESTATUS clause
IF(@FILESTATUS IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @WHERE = @WHERE + ' F.fileStatus = @FILESTATUS '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileStatus = @FILESTATUS '
	END
END
	
--- @INTERACTIVE clause
IF(@INTERACTIVE IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @WHERE = @WHERE + ' F.fileAllowExternal = @INTERACTIVE '
	END
	ELSE
	BEGIN
		SET @WHERE = @WHERE + ' AND F.fileAllowExternal = @INTERACTIVE '
	END
END

--- create the final where clause
IF(@WHERE <> '')
BEGIN
	set @WHERE = N' WHERE ' + @WHERE
END

--- BUILD THE SQL
SET @SQL = Rtrim(@SELECT) + Rtrim(@WHERE)

-- Debug Print
-- print @SQL

exec sp_executesql @sql,
N'
	@CONTNAME nvarchar(128),
	@CONTTYPE nvarchar(15),
	@FILESTATUS nvarchar(15),
	@INTERACTIVE bit,
	@SUBCONTTYPE nvarchar(15),	
	@UI nvarchar(10)',
	@CONTNAME,
	@CONTTYPE,
	@FILESTATUS,
	@INTERACTIVE,
	@SUBCONTTYPE,
	@UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConAssocs] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepConAssocs] TO [OMSAdminRole]
    AS [dbo];

