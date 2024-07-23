

CREATE PROCEDURE [dbo].[schSearchDocText] (@FILEID bigint = null,@MAX_RECORDS int = 50,  @TEXT nvarchar(100) = '', @CREATEDBY int = null)  
AS


declare @Select nvarchar(1900)
declare @Top nvarchar(10)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

SET @WHERE = ''

if @MAX_RECORDS > 0
	set @Top = N'TOP ' + Convert(nvarchar, @MAX_RECORDS)
else
	set @Top = N''


if @TEXT <> ''  AND @TEXT is not null 
	BEGIN 
		set @select = N'
		SELECT ' + @Top + N' 
			dbo.dbUser.usrInits AS CrByInits, dbo.dbUser.usrFullName AS CrByFullName, dbUser_1.usrInits AS UpdByInits, 
		             dbUser_1.usrFullName AS UpdByFullName, dbo.dbDocument.*, COALESCE(dbDocument.docAuthored, dbDocument.Created) AS docCreated, DocWordCount, Characterization, Rank
		FROM OPENQUERY(DocInd,
		               ''SET PROPERTYNAME ''''d5cdd505-2e9c-101b-9397-08002b2cf9ae''''
				    PROPID ''''PRECid'''' AS preclid 
				    ;
			SET PROPERTYNAME ''''d5cdd505-2e9c-101b-9397-08002b2cf9ae''''
				    PROPID ''''docid'''' AS docdocid 
				    ;
			SELECT Rank,precid,docthumbnail,characterization,DocWordCount 
			                FROM SCOPE('''' "D:\Development" '''') WHERE Contains('''' ' + @TEXT + ' '''') 
               
		               '') AS Q, dbo.dbUser dbUser_1 RIGHT OUTER JOIN
			                   config.dbDocument ON dbUser_1.usrID = dbDocument.UpdatedBy 
								LEFT JOIN	[config].[DocumentAccess] () r on r.DocumentID = dbDocument.docID and r.[deny] is not null
							   LEFT OUTER JOIN
			                   dbo.dbUser ON dbDocument.Createdby = dbo.dbUser.usrID 
			  where Q.docdocid = dbdocument.docid'

		if @FILEID is not null	
			BEGIN			
				set @where = N' AND FILEID = @FILEID'
				IF not @CREATEDBY is null
					BEGIN 
						set @where = @where + N' AND dbdocument.CreatedBy = @CREATEDBY'
					END
			END	
		else
			BEGIN
				IF not @CREATEDBY is null
					BEGIN 
						set @where = @where + N' AND dbdocument.CreatedBy = @CREATEDBY'
					END
			END
		
		set @orderby = N' ORDER BY Rank'
	end
ELSE
	BEGIN
		set @select = N'
		DECLARE @T table (docid bigint NOT NULL PRIMARY KEY (docid) )
		insert into @T select Docid from config.dbDocument doc left join [config].[DocumentAccess] () deny1 on doc.docid = deny1.DocumentID and deny1.[deny] is not null where deny1.DocumentID is null

		SELECT ' + @Top + N' 
			dbo.dbUser.usrInits AS CrByInits, dbo.dbUser.usrFullName AS CrByFullName, dbUser_1.usrInits AS UpdByInits, 
		             dbUser_1.usrFullName AS UpdByFullName, dbo.dbDocument.*, COALESCE(dbDocument.docAuthored, dbDocument.Created) AS docCreated
		FROM dbo.dbUser dbUser_1 RIGHT OUTER JOIN
			                   config.dbDocument ON dbUser_1.usrID = dbDocument.UpdatedBy 
								LEFT JOIN	[config].[DocumentAccess] () r on r.DocumentID = dbDocument.docID and r.[deny] is not null
							   LEFT OUTER JOIN
			                   dbo.dbUser ON dbDocument.Createdby = dbo.dbUser.usrID '
		if @FILEID is not null	
			BEGIN			
				set @where = N' AND FILEID = @FILEID'
				IF not @CREATEDBY is null
					BEGIN 
						set @where = @where + N' AND dbdocument.CreatedBy = @CREATEDBY'
					END
			END	
		else
			BEGIN
				IF not @CREATEDBY is null
					BEGIN 
						set @where = @where + N' AND dbdocument.CreatedBy = @CREATEDBY'
					END
			END

	END			


declare @sql nvarchar(4000)
set @sql = @select + @where 

exec sp_executesql @sql,  N'@FILEID bigint,@MAX_RECORDS int,  @TEXT nvarchar(100), @CREATEDBY int ', @FILEID, @MAX_RECORDS, @TEXT, @CREATEDBY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocText] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchDocText] TO [OMSAdminRole]
    AS [dbo];

