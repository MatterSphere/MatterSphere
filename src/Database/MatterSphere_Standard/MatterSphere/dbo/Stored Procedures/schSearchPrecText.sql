
CREATE PROCEDURE [dbo].[schSearchPrecText] (@MAX_RECORDS int = 50,  @TEXT nvarchar(100) = '', @CREATEDBY int = null)  
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
		if SUBSTRING(@TEXT,1,1) = '"'
			BEGIN
		
			set @select = N'
				SELECT ' + @Top + N' *
				FROM OpenRowset(''MSIDXS'',''Precedent'';''''; '''',  ''
					SET PROPERTYNAME ''''d5cdd505-2e9c-101b-9397-08002b2cf9ae''''
								    PROPID ''''precid'''' AS precid 
							    ;
			
					Select precid,Rank,docthumbnail,characterization, DocWordCount, Directory, FileName, size, Create, Write
						 from SCOPE() where contains('''' ' + @TEXT + ' '''') > 0 
					 '')
					 AS a inner join dbprecedents on dbprecedents.precid = a.precid
				'
			END
		ELSE
			BEGIN

				set @select = N'
					SELECT ' + @Top + N' *
					FROM OpenRowset(''MSIDXS'',''Precedent'';''''; '''',  ''
						SET PROPERTYNAME ''''d5cdd505-2e9c-101b-9397-08002b2cf9ae''''
									    PROPID ''''precid'''' AS precid 
								    ;
				
						Select precid,Rank,docthumbnail,characterization, DocWordCount, Directory, FileName, size, Create, Write
							 from SCOPE() where contains('''' "' + @TEXT + '" '''') > 0 
						 '')
						 AS a inner join dbprecedents on dbprecedents.precid = a.precid
						'
			END
	end
ELSE
	BEGIN
		set @select = N'
			SELECT ' + @Top + N' *
			FROM OpenRowset(''MSIDXS'',''Precedent'';''''; '''',  ''
				SET PROPERTYNAME ''''d5cdd505-2e9c-101b-9397-08002b2cf9ae''''
					PROPID ''''precid'''' AS precid 
				   ;
				Select precid,Rank,docthumbnail,characterization, DocWordCount, Directory, FileName, size, Create, Write
					 from SCOPE() 
				 '')
				 AS a inner join dbprecedents on dbprecedents.precid = a.precid
			'

	END			

set @orderby = N' ORDER BY Rank'

declare @sql nvarchar(4000)
set @sql = @select + @where 
print @sql

exec sp_executesql @sql

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchPrecText] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchPrecText] TO [OMSAdminRole]
    AS [dbo];

