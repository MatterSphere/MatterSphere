

CREATE PROCEDURE [dbo].[srepLAMatNoCertDate]
(@UI uUICultureInfo='{default}')

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'SELECT     dbo.dbClient.clNo, dbo.dbFile.fileNo, dbo.dbClient.clName, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, dbo.dbFile.filePrincipleID, dbo.dbUser.usrFullName, 
                      dbo.dbUser.usrInits, dbo.dbFile.fileFundRef, dbo.dbFile.fileFundCode, dbo.dbFile.fileAgreementDate, dbo.dbFile.fileStatus, dbo.dbFile.fileLACategory, 
                      dbo.dbFundType.ftLegalAidCharged
FROM         dbo.dbClient INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID AND dbo.dbfile.fileagreementdate IS NULL AND dbo.dbfile.filestatus LIKE ''LIVE'' INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID INNER JOIN
                      dbo.dbFundType ON dbo.dbFile.fileFundCode = dbo.dbFundType.ftCode AND dbo.dbFundType.ftLegalAidCharged = ''1'' '

---- Debug Print
PRINT @SELECT

---- Build Where Clause
SET @WHERE = ''

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

---- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)
--- Add Statements together
set @sql = @select + @where + @orderby

--- Debug Print
print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLAMatNoCertDate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepLAMatNoCertDate] TO [OMSAdminRole]
    AS [dbo];

