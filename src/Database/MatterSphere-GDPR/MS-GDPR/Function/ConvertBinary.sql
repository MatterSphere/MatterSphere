CREATE FUNCTION [dbo].[ConvertBinary]
(
    @value AS binary(34),
	@byte as int,
	@newvalue as int
) RETURNS binary(34) AS 
BEGIN  

    DECLARE @result AS binary(34),
			@result1 AS binary(1),
            @result2 AS varchar(max),
            @idx AS int;

    SELECT @result = @value;  
    SELECT @result2 = '';
    SELECT @idx = 1;

    WHILE @idx < len(@result)
        BEGIN
			set @result1 = substring(@result,@idx,1)
			if @idx = @byte 
				set @result1 = @result1+@newvalue
			SET @result2 = @result2 + CONVERT(varchar(8),@result1,2)

            SET @idx = @idx + 1;
        END
    return CONVERT(binary(34), @result2, 2);  

END 
GO