

CREATE PROCEDURE [dbo].[srepKeyDates]
(
	@DESCRIPTION nvarchar(15) = null,
	@FEEEARNER bigint = null,
	@STATUS nvarchar(15) = null,
	@UI uUICultureInfo='{default}',
	@STARTDATE datetime = null,
	@ENDDATE datetime = null,
	@ACTIVEKD bit = null,
	@INACTIVEKD bit = null
)
AS 

declare @sql nvarchar(4000)
declare @Select nvarchar(1900)
declare @Where nvarchar(1000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
SET @SELECT = N'
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT  
	CL.clNo + ''/'' + F.fileNo as Ref,  
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	KD.kdDesc, 
	KD.kdDate,
	CASE
		WHEN KD.kdActive = 1 THEN ''Active''
		ELSE ''Inactive''
	END AS KdStatus,
	U.usrFullName
FROM         
	dbo.dbFile F
INNER JOIN	
	dbo.dbClient CL ON F.clID = CL.clID 
INNER JOIN
	dbo.dbKeyDates KD ON F.fileID = KD.fileID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID'

--- Build the WHERE statement
SET @where = ''

--- @STARTDATE filter
IF(@STARTDATE IS NOT NULL)
BEGIN
	SET @where = @where + ' WHERE (KD.kdDate >= @STARTDATE AND KD.kdDate < @ENDDATE)'
END

--- @DESCRIPTION filter
IF(@DESCRIPTION IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @where = @where + ' WHERE KD.kdType = @DESCRIPTION'
	END
	ELSE
	BEGIN
		SET @where = @where + ' AND KD.kdType = @DESCRIPTION'
	END
END

---@FEEEARNER Filter
IF(@FEEEARNER IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @where = @where + ' WHERE F.filePrincipleID = @FEEEARNER'
	END
	ELSE
	BEGIN
		SET @where = @where + ' AND F.filePrincipleID = @FEEEARNER'
	END
END

---@STATUS Filter
IF(@STATUS IS NOT NULL)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @where = @where + ' WHERE F.fileStatus = @STATUS'
	END
	ELSE
	BEGIN
		SET @where = @where + ' AND F.fileStatus = @STATUS'
	END
END

-- Active Key Dates filter
IF (@ACTIVEKD = 1)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @where = @where + ' WHERE KD.kdActive = 1'
	END
	ELSE
	BEGIN
		SET @where = @where + ' AND KD.kdActive = 1'
	END
END

-- Inactive Key Dates filter
IF (@INACTIVEKD = 1)
BEGIN
	IF(@WHERE = '')
	BEGIN
		SET @where = @where + ' WHERE KD.kdActive = 0'
	END
	ELSE
	BEGIN
		SET @where = @where + ' AND KD.kdActive = 0'
	END
END

--- Build the ORBER BY clause
SET @orderby = N' ORDER BY KD.kdDate DESC'		

--- Now build the whole statement
SET @sql = @select + @where + @orderby

-- Debug Print
-- PRINT @sql

--- Lets go
exec sp_executesql @sql, 
N'
	@DESCRIPTION nvarchar(15),
	@FEEEARNER bigint,
	@STATUS nvarchar(15),
	@UI nvarchar(10),
	@STARTDATE datetime,
	@ENDDATE datetime,
	@ACTIVEKD bit,
	@INACTIVEKD bit',
	@DESCRIPTION,
	@FEEEARNER,
	@STATUS,
	@UI,
	@STARTDATE,
	@ENDDATE,
	@ACTIVEKD,
	@INACTIVEKD


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepKeyDates] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepKeyDates] TO [OMSAdminRole]
    AS [dbo];

