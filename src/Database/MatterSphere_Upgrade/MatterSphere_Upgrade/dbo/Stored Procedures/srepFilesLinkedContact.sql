CREATE PROCEDURE [dbo].[srepFilesLinkedContact]
(@UI uUICultureInfo='{default}',
@ClNo nvarchar(50) = null,
@ContType nvarchar(50) = null,
@SubContType nvarchar(50) = null,
@ContName nvarchar(128) = null,
@MatterStat nvarchar(50) = null,
@FileAllowExternal bit = null)

AS 

declare @Select nvarchar(max)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
DECLARE @T TABLE (cdCode nvarchar(15), cdDesc nvarchar(1000))
INSERT INTO @T 
SELECT cdCode, cdDesc FROM dbo.GetCodeLookupDescription ( ''COUNTRIES'', @UI ) 

SELECT     dbo.dbContact.contName, dbo.dbAddress.addLine1, dbo.dbAddress.addLine2, dbo.dbAddress.addLine3, dbo.dbAddress.addLine4, 
                      dbo.dbAddress.addLine5, dbo.dbAddress.addCountry, dbo.dbAddress.addPostcode, dbo.dbClient.clNo, dbo.dbFile.fileNo, dbo.dbContact.contTypeCode, 
                      dbo.dbContact.contAddFilter, replace(dbo.dbFile.fileDesc, char(13) + char(10), '', '') as fileDesc, dbo.dbFile.filePrincipleID, dbo.dbUser.usrInits, dbo.dbClient.clName, dbo.dbFile.fileStatus, 
                      dbo.dbFile.fileAllowExternal, dbo.dbCodeLookup.cdDesc, dbo.dbCodeLookup.cdCode, dbo.dbCountry.ctryCode, 
                      COALESCE(CL1.cdDesc, ''~'' + NULLIF(dbCountry.ctryCode, '''') + ''~'') AS CountryDesc
FROM         dbo.dbContact INNER JOIN
                      dbo.dbAddress ON dbo.dbContact.contDefaultAddress = dbo.dbAddress.addID INNER JOIN
                      dbo.dbClient ON dbo.dbContact.contID = dbo.dbClient.clDefaultContact INNER JOIN
                      dbo.dbFile ON dbo.dbClient.clID = dbo.dbFile.clID INNER JOIN
                      dbo.dbUser ON dbo.dbFile.filePrincipleID = dbo.dbUser.usrID INNER JOIN
                      dbo.dbCodeLookup ON dbo.dbContact.contTypeCode = dbo.dbCodeLookup.cdCode INNER JOIN
                      dbo.dbCountry ON dbo.dbAddress.addCountry = dbo.dbCountry.ctryID
			LEFT JOIN @T CL1 ON CL1.[cdCode] = dbCountry.ctryCode'

---- Build Where Clause
SET @WHERE = ''


if coalesce(@ClNo, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbclient.clno = ''' + @ClNo + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbclient.clno = ''' + @ClNo + ''''
	END
END

if coalesce(@ContType, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbcodelookup.cddesc = ''' + @ContType + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbcodelookup.cddesc = ''' + @conttype + ''''
	END
END

if coalesce(@SubContType, '') <> ''
BEGIN
   IF @where <> '' 
	BEGIN
	     set @where = @where + ' AND dbo.dbcodelookup.cdcode = ''' + @SubContType + ''''
	END
   ELSE
	BEGIN
	     set @where = @where + ' dbo.dbcodelookup.cdcode = ''' + @SubContType + ''''
	END
END

if coalesce(@ContName, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbcontact.contname = ''' + @contname + '''' 
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbcontact.contname = ''' + @contname + '''' 
	END
END

if coalesce(@MatterStat, '') <> ''
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.filestatus = ''' + @matterstat + ''''
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.filestatus = ''' + @matterstat + ''''
	END
END

if not @fileallowexternal is null
BEGIN
   IF @where <> ''
	BEGIN
	    set @where = @where + ' AND dbo.dbfile.fileallowexternal = ' + convert(nvarchar, @fileallowexternal)
	END
   ELSE
	BEGIN
	    set @where = @where + ' dbo.dbfile.fileallowexternal = ' + convert(nvarchar, @fileallowexternal)
	END
END

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
    ON OBJECT::[dbo].[srepFilesLinkedContact] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepFilesLinkedContact] TO [OMSAdminRole]
    AS [dbo];

