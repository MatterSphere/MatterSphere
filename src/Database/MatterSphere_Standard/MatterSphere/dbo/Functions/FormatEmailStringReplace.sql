

CREATE FUNCTION[dbo].[FormatEmailStringReplace] ( @inString nvarchar(max) ,@DocID bigint)
RETURNS nvarchar(max)
AS
BEGIN
        DECLARE @result nvarchar(max)
		DECLARE @FileNo nvarchar(25)
		DECLARE @ClNo nvarchar(25)

		select 
			@FileNo = f.fileNo,
			@ClNo = c.clNo 
		from config.dbDocument d 
		join config.dbFile f on d.fileID = f.fileID
		join config.dbClient c on f.clID = c.clID
		where d.docID = @DocID;		
		
        SELECT @inString = REPLACE(@inString,'{CLNO}',@ClNo)
		SELECT @inString = REPLACE(@inString,'{FileNo}',@FileNo)
		SELECT @inString = REPLACE(@inString,'{docID}',@DocID)

		RETURN @inString
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FormatEmailStringReplace] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FormatEmailStringReplace] TO [OMSAdminRole]
    AS [dbo];

