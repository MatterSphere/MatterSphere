

CREATE PROCEDURE [dbo].[srepMilestoneManager]
(@UI uUICultureInfo='{default}',
@user nvarchar(50) = null,
@begindate datetime=null,
@enddate datetime=null)
AS 

declare @Select nvarchar(2700)
declare @Where nvarchar(1200)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     
	dbo.dbClient.clNo, 
	dbo.dbClient.clName, 
	dbo.dbAddress.addLine1, 
	dbo.dbFile.fileNo, 
	replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	dbo.dbMSData_OMS2K.MSNextDueDate, 
       	dbo.dbMSConfig_OMS2K.MSDescription, 
	dbo.dbUser.usrFullName, 
	CONVERT(int, dbo.dbMSData_OMS2K.MSNextDueDate) AS DUE, 
	CONVERT(int, { fn NOW() }) AS TODAY, 
	CONVERT(int, dbo.dbMSData_OMS2K.MSNextDueDate) - CONVERT(int, { fn NOW() }) + 1 AS DATEDIFFERENCE, 
        CASE 
	WHEN CONVERT(int, dbo.dbMSData_OMS2K.MSNextDueDate) - CONVERT(int, { fn NOW() }) + 1 > 0 THEN ''Due in'' 
	WHEN CONVERT(int, dbo.dbMSData_OMS2K.MSNextDueDate) - CONVERT(int, { fn NOW() }) + 1 < 0 THEN ''Days Overdue'' 
	WHEN CONVERT(int, dbo.dbMSData_OMS2K.MSNextDueDate) - CONVERT(int, { fn NOW() }) + 1 = 0 THEN ''Due Today'' 
	END AS DateDiffDesc, 
	dbo.dbFile.fileType, 
        CASE 
	WHEN MSNEXTDUESTAGE = 1 THEN MSSTAGE1DESC 
	WHEN MSNEXTDUESTAGE = 2 THEN MSSTAGE2DESC 
	WHEN MSNEXTDUESTAGE = 3 THEN MSSTAGE3DESC 
	WHEN MSNEXTDUESTAGE = 4 THEN MSSTAGE4DESC 
	WHEN MSNEXTDUESTAGE = 5 THEN MSSTAGE5DESC 
	WHEN MSNEXTDUESTAGE = 6 THEN MSSTAGE6DESC 
	WHEN MSNEXTDUESTAGE = 7 THEN MSSTAGE7DESC 
	WHEN MSNEXTDUESTAGE = 8 THEN MSSTAGE8DESC 
	WHEN MSNEXTDUESTAGE = 9 THEN MSSTAGE9DESC 
	WHEN MSNEXTDUESTAGE = 10 THEN MSSTAGE10DESC 
	WHEN MSNEXTDUESTAGE = 11 THEN MSSTAGE11DESC 
	WHEN MSNEXTDUESTAGE = 12 THEN MSSTAGE12DESC 
	WHEN MSNEXTDUESTAGE = 13 THEN MSSTAGE13DESC 
	WHEN MSNEXTDUESTAGE = 14 THEN MSSTAGE14DESC 
	WHEN MSNEXTDUESTAGE = 15 THEN MSSTAGE15DESC 
	WHEN MSNEXTDUESTAGE = 16 THEN MSSTAGE16DESC 
	WHEN MSNEXTDUESTAGE = 17 THEN MSSTAGE17DESC
	WHEN MSNEXTDUESTAGE = 18 THEN MSSTAGE18DESC 
	WHEN MSNEXTDUESTAGE = 19 THEN MSSTAGE19DESC 
	WHEN MSNEXTDUESTAGE = 20 THEN MSSTAGE20DESC 
	WHEN MSNEXTDUESTAGE = 21 THEN MSSTAGE21DESC 
	WHEN MSNEXTDUESTAGE = 22 THEN MSSTAGE22DESC 
	WHEN MSNEXTDUESTAGE = 23 THEN MSSTAGE23DESC 
	WHEN MSNEXTDUESTAGE = 24 THEN MSSTAGE24DESC 
	WHEN MSNEXTDUESTAGE = 25 THEN MSSTAGE25DESC 
	WHEN MSNEXTDUESTAGE = 26 THEN MSSTAGE26DESC 
	WHEN MSNEXTDUESTAGE = 27 THEN MSSTAGE27DESC
	WHEN MSNEXTDUESTAGE = 28 THEN MSSTAGE28DESC 
	WHEN MSNEXTDUESTAGE = 29 THEN MSSTAGE29DESC 
	WHEN MSNEXTDUESTAGE = 30 THEN MSSTAGE30DESC 
	ELSE ''OTHER!'' 
	END AS StageDesc, 
	dbo.dbMSData_OMS2K.MSNextDueStage, 
	dbo.dbuser.usrinits
FROM         
	dbo.dbMSData_OMS2K 
INNER JOIN
	dbo.dbAddress 
INNER JOIN
	dbo.dbClient ON dbo.dbAddress.addID = dbo.dbClient.clDefaultAddress 
INNER JOIN
	dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID ON dbo.dbMSData_OMS2K.fileID = dbo.dbFile.fileID 
INNER JOIN
	dbo.dbMSConfig_OMS2K ON dbo.dbMSData_OMS2K.MSCode = dbo.dbMSConfig_OMS2K.MSCode 
INNER JOIN
	dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID'

--- Build Where Clause
SET @WHERE = ''

if coalesce(@user, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbuser.usrid = ''' + @user + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbuser.usrid = ''' + @user + ''''
	END
END

if coalesce(@BEGINDATE, '') <> ''
BEGIN
   IF @where is null
	BEGIN
	    if coalesce(@ENDDATE, '') <> ''
		BEGIN
		    set @where = @where + ' dbo.dbdbmsdata_oms2k.msnextduedate BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' dbo.dbmsdata_oms2k.msnextduedate BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
   ELSE
	BEGIN
	    if @ENDDATE is null
		BEGIN
		    set @where = @where + ' AND dbo.dbmsdata_oms2k.msnextduedate BETWEEN '''  + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@BEGINDATE + 1) + ''''	
		END
	    ELSE
		BEGIN
		    set @where = @where + ' AND dbo.dbmsdata_oms2k.msnextduedate BETWEEN ''' + Convert(nvarchar(50),@BEGINDATE) + ''' AND ''' + Convert(nvarchar(50),@ENDDATE + 1) + ''''
		END
	END
END  

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMilestoneManager] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepMilestoneManager] TO [OMSAdminRole]
    AS [dbo];

