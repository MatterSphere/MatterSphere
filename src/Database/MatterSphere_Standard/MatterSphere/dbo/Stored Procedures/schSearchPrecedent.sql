

CREATE PROCEDURE [dbo].[schSearchPrecedent]  (@USRID nvarchar(32) = '0', @TYPE uCodeLookup = '',@CAT uCodelookup = '',  @SUBCAT uCodeLookup = '', @MINORCAT uCodeLookup = '', @ADDRESSEE nvarchar(50) = '', 
@ASSOCTYPE uCodeLookup = '', @LIBRARY uCodeLookup = '', @MS_PLAN uCodeLookup = '', @MS_STAGE int = null, @LANGUAGE uUICultureInfo = '', @UI uUICultureInfo = '{default}', @FavOnly BIT = 0)  
AS
declare @Select nvarchar(MAX)
declare @Where nvarchar(MAX), @usridint int = CAST(@USRID AS int)
if @CAT is null set @CAT = ''
if @SUBCAT is null set @SUBCAT = ''
if @MINORCAT is null set @MINORCAT = ''
if @ADDRESSEE is null set @ADDRESSEE = ''
if @TYPE  is null set @TYPE = ''
if @LIBRARY  is null set @LIBRARY = ''
if @LANGUAGE  is null set @LANGUAGE = ''

set @select = N' DECLARE @Fav TABLE (PrecID BIGINT PRIMARY KEY, FavID INT)

			INSERT INTO @Fav (PrecID, FavID)
			SELECT CAST(usrFavObjParam1 AS BIGINT)
				, FavID
			FROM dbo.dbUserFavourites 
			WHERE usrFavType = ''PRECFAV'' 
				AND usrFavDesc = ''PRECFAVTAB''
				AND usrID = @usridint

			SELECT 
			PrecTitle,p.PrecID,PrecDesc,PrecCategory,PrecSubCategory,PrecMinorCategory,PrecAddressee,PrecType, precLanguage,precmultiprec
			,COALESCE(CL1.cdDesc, ''~'' + NULLIF(p.prectype, '''') + ''~'') as PrecTypeDesc
			,COALESCE(CL2.cdDesc, ''~'' + NULLIF(p.preccategory, '''') + ''~'') as PrecCategoryDesc
			,COALESCE(CL3.cdDesc, ''~'' + NULLIF(p.precsubcategory, '''') + ''~'') as PrecSubCategoryDesc 
			,COALESCE(CL6.cdDesc, ''~'' + NULLIF(p.precminorcategory, '''') + ''~'') as PrecMinorCategoryDesc 
			,COALESCE(CL4.cdDesc, ''~'' + NULLIF(p.preclibrary, '''') + ''~'') as PrecLibraryDesc
			,COALESCE(CL5.cdDesc, ''~'' + NULLIF(p.precaddressee, '''') + ''~'') as PrecAddresseeDesc
			,p.precCheckedOut, p.precCheckedOutBy, p.precCheckedOutlocation, p.precExtension
			, UF.FavID
			from DBPrecedents p
			LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''DOCTYPE'', @ui ) CL1 ON CL1.[cdCode] =  p.prectype
			LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''PRECCAT'', @ui ) CL2 ON CL2.[cdCode] = p.preccategory
			LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''PRECSUBCAT'', @ui ) CL3 ON CL3.[cdCode] = p.precsubcategory
			LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''PRECMINORCAT'', @ui ) CL6 ON CL6.[cdCode] = p.precminorcategory
			LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''PRECLIBRARY'', @ui ) CL4 ON CL4.[cdCode] = p.preclibrary
			LEFT JOIN [dbo].[GetCodeLookupDescription] ( ''CONTTYPE'', @ui ) CL5 ON CL5.[cdCode] = p.precaddressee
			LEFT JOIN dbPrecedentTeamsAccess pta on p.PrecID = pta.precID
		    LEFT JOIN dbTeam t on t.tmID = pta.teamId
            LEFT JOIN dbTeamMembership tm on t.tmID = tm.tmID
            LEFT JOIN dbUser u on u.usrID = tm.usrID
			LEFT JOIN @Fav UF ON UF.PrecID = p.PrecID '
set @where = ' WHERE (u.usrId = @usridint OR pta.precId IS NULL)'
if @CAT != '' or @SUBCAT != '' or @MINORCAT != '' or @ADDRESSEE != '' or @ASSOCTYPE != '' or @TYPE != '' or @LIBRARY != '' or @LANGUAGE != '' or @MS_PLAN != ''
	BEGIN
		if @LANGUAGE != ''
		BEGIN
			set @where = @where + N' AND p.precid = dbo.GetPrecedent(prectitle, prectype, preclibrary, preccategory, precsubcategory, precminorcategory, @Language) ' 
		END		

		IF @CAT != '' 
		BEGIN
			set @Where = @Where + N' AND PrecCategory = @CAT '
		END

		IF @SUBCAT != '' 
		BEGIN			
			set @Where = @Where + N' AND PrecSubCategory = @SUBCAT '
		END

		IF @MINORCAT != '' 
		BEGIN
			set @Where = @Where + N' AND PrecMinorCategory = @MINORCAT '				
		END	

		---IF @MS_PLAN != ''
		---BEGIN
			---set @where = @where + N' AND (PrecMSCode = @MS_PLAN or PrecMSCode is Null)  '
			if not @MS_STAGE is null
			begin
				set @where = @where + N' AND (PrecMS = @MS_STAGE or PrecMS = 0)  '
			end
			
		---END

		IF @LIBRARY != ''
		BEGIN
			set @where = @where + N' AND (PRECLIBRARY = @LIBRARY)  '
		END
		ELSE
		BEGIN
			set @where = @where + N' AND (PRECLIBRARY <> ''ARCHIVE'' OR PRECLIBRARY is null)  '
		END
		IF @ADDRESSEE != '' 
		BEGIN
			set @where = @where + N' AND (PRECADDRESSEE = @ADDRESSEE or   PRECADDRESSEE is null)'
			if @ASSOCTYPE != ''
			begin
				set @where = @where + N' AND (PRECASSOCTYPE = @ASSOCTYPE or   PRECASSOCTYPE is null)'
			end
		END

		IF @TYPE != '' 
		BEGIN
			set @where = @where + N' AND PRECTYPE = @TYPE  '
		END
	END
ELSE
	BEGIN
		set @where = @where + N' AND (PRECLIBRARY <> ''ARCHIVE'' OR PRECLIBRARY is null)'
	END
IF @FavOnly = 1
	SET @where = @where + N' AND UF.FavID IS NOT NULL'

declare @sql nvarchar(MAX)
set @sql = @select + @where + N' ORDER BY prectitle'
print @sql
exec sp_executesql @sql,N'@usridint int, @TYPE uCodeLookup, @CAT uCodeLookup, @SUBCAT uCodeLookup, @MINORCAT uCodeLookup, @ADDRESSEE uCodeLookup, @ASSOCTYPE uCodeLookup, @LIBRARY uCodeLookup , @MS_PLAN uCodeLookup, @MS_STAGE int, @LANGUAGE uUICultureInfo, @UI uUICultureInfo, @FavOnly BIT',@usridint,@TYPE,@CAT,@SUBCAT,@MINORCAT,@ADDRESSEE, @ASSOCTYPE, @LIBRARY, @MS_PLAN, @MS_STAGE, @LANGUAGE,@UI, @FavOnly
GO

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchPrecedent] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchPrecedent] TO [OMSAdminRole]
    AS [dbo];

