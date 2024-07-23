
CREATE FUNCTION [dbo].[Decr] ( @data nvarchar(1000) )
RETURNS nvarchar(1000) 

AS  
BEGIN 
	DECLARE @ret nvarchar(1000) , @a int , @key int 
	SET @ret = ''			
	SET @a = unicode(left(@data, 1))
	SET @key = unicode(right(@data, 1))
	SET @data = substring(@data, 2, @key)
	SET @key = @key + @a

	DECLARE @ctr int
	SET @ctr = len(@data)
	WHILE (@ctr > 0)
	BEGIN
		DECLARE @ach int
		SET @ach = unicode(substring(@data, @ctr, 1))
		SET @ach = @ach - @key	

		IF (@ach < 1) 
		BEGIN
			WHILE @ach < 1
			BEGIN
				SET @ach = @ach + 255
			END		
		END
		SET @ret = @ret + char(@ach)
		SET @ctr = @ctr - 1
	END
	RETURN @ret
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Decr] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Decr] TO [OMSAdminRole]
    AS [dbo];

