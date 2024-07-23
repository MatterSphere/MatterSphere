

CREATE PROCEDURE [dbo].[schSearchClientFile] (@MAX_RECORDS int = 50,  @SEARCH nvarchar(128) = '', @ADDRESS nvarchar(150) = '', @CLTYPE uCodeLookup = null, @FILEDESC nvarchar(200) = '', @FILETYPE uCodeLookup = null, @FILEPRINCIPLEID bigint = null,
 @DEBUG bit = 0, @FILEDEPT uCodeLookup = null,@SOUNDEX bit = 0)  
AS

declare @Select nvarchar(1900)
declare @Top nvarchar(10)
declare @Where nvarchar(2000)

if @MAX_RECORDS > 0
	set @Top = N'TOP ' + Convert(nvarchar, @MAX_RECORDS)
else
	set @Top = N''

set @select = N'SELECT ' + @Top + N' 
			CL.clNo, 
			CL.clName, 
			F.fileNo, 
			F.fileDesc, 
			F.fileID, 
			F.clID, 
			CONT.contName,
			replace(replace(coalesce(ADR.addLine1,'''') + '', '' + coalesce(ADR.addLine2,'''') + '', '' + coalesce(ADR.addLine3,'''') + '', '' + coalesce(ADR.addLine4,'''') + '', '' + coalesce(ADR.addLine5,'''') + '', '' + coalesce(ADR.addPostCode,'''') ,'', , '' 
 ,  '', '') ,'', , ''  ,  '', '') as ConcatAddress,
			U.usrInits as filePrincipleInits,
			FT.typeGlyph,
			F.fileStatus
		FROM
			dbClient CL
		INNER JOIN
			dbFile F ON F.clID = CL.clID
		INNER JOIN
			dbContact CONT ON CL.clDefaultContact = CONT.contID
		INNER JOIN
			dbAddress ADR ON ADR.addID = CONT.contDefaultAddress 
		INNER JOIN 
			dbUser U on U.usrID = F.filePrincipleID
		INNER JOIN
			dbFileType FT on FT.typeCode = F.fileType'

set @where = N' WHERE CL.clTypeCode = coalesce(@CLTYPE, CL.clTypeCode) AND F.filePrincipleID = coalesce(@FILEPRINCIPLEID, F.filePrincipleID) AND F.FileDepartment = coalesce(@FILEDEPT, F.FileDepartment) '

IF @SEARCH <> '' and @SEARCH is not null
BEGIN

   if @SOUNDEX = 1 
	
	set @where = @where + 
		N'		AND 
				(
					( SOUNDEX ( CL.clSearch1 ) = SOUNDEX ( @SEARCH ) ) OR
					( SOUNDEX ( CL.clSearch2 ) = SOUNDEX ( @SEARCH ) ) OR
					( SOUNDEX ( CL.clSearch3 ) = SOUNDEX ( @SEARCH ) ) OR
					( SOUNDEX ( CL.clSearch4 ) = SOUNDEX ( @SEARCH ) ) OR
					( SOUNDEX ( CL.clSearch5 ) = SOUNDEX ( @SEARCH ) ) )'
	
	else
	set @where = @where + 
		N' AND 
		(
			CL.CLName like ''%'' + @SEARCH + ''%'' 
		OR
		(
			CL.CLSearch1 = @SEARCH OR
			CL.CLSearch2 = @SEARCH OR
			CL.CLSearch3 = @SEARCH OR
			CL.CLSearch4 = @SEARCH OR
			CL.CLSearch5 = @SEARCH 
		)
		) '

END	
IF @ADDRESS <> '' and @ADDRESS is not null
BEGIN
	set @where = @where + N' AND (
				(ADR.addLine1 like ''%'' + @ADDRESS + ''%'') OR
				(ADR.addLine2 like ''%'' + @ADDRESS + ''%'') OR
				(ADR.addLine3 like ''%'' + @ADDRESS + ''%'') OR
				(ADR.addLine4 like ''%'' + @ADDRESS + ''%'') OR
				(ADR.addLine5 like ''%'' + @ADDRESS + ''%'') OR
				(ADR.addPostCode like ''%'' + @ADDRESS + ''%'')) '
END

if (datalength(@FILEDESC) > 0)
	set @where = @where + N' AND (F.fileDesc LIKE ''%'' + @FILEDESC + ''%'')'

if not @CLTYPE is null
	set @where = @where + N' AND (CL.cltypecode = @CLTYPE)'

if not @FILETYPE is null
	set @where = @where + N' AND (F.fileType = @FILETYPE)'


declare @sql nvarchar(4000)
set @sql = @select + @where + 'ORDER BY CL.CLNO, F.FILENO'

if @debug = 1 print @sql
exec sp_executesql @sql,  N'@SEARCH nvarchar(128), @ADDRESS nvarchar(150), @CLTYPE uCodeLookup, @FILEDESC nvarchar(200), @FILETYPE uCodeLookup, @FILEPRINCIPLEID bigint, @FILEDEPT ucodelookup', @SEARCH, @ADDRESS, 
@CLTYPE, @FILEDESC, @FILETYPE, @FILEPRINCIPLEID, @FILEDEPT

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchClientFile] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchClientFile] TO [OMSAdminRole]
    AS [dbo];

