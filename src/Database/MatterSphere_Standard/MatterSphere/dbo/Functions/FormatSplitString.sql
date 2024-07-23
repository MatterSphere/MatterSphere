

CREATE FUNCTION[dbo].[FormatSplitString] ( @inString nvarchar(400))
RETURNS varchar(400)
AS
BEGIN
        DECLARE @result nvarchar(500) , @word nvarchar(400), @char nchar(1)
        SET @instring =  Rtrim ( Ltrim ( @instring ) )
        SET @result = ''
        WHILE LEN ( @inString ) > 0
        BEGIN
                    SET @char = Left (@inString , 1)
                    IF ASCII(@char) BETWEEN 65 AND 90
                                SET @result = @result + ' ' + @char
                    ELSE
                                SET @result = @result + @char
                    SET @instring = SUBSTRING(@instring, 2, 50)
                                IF LEN (@inString) = 0
                                BREAK
        END
        
        RETURN Ltrim(@result)
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FormatSplitString] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FormatSplitString] TO [OMSAdminRole]
    AS [dbo];

