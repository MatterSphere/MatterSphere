

CREATE FUNCTION [config].[GetPolicyMaskValue] ()
RETURNS varbinary(16)

AS
BEGIN
	DECLARE @x tinyint , @y varbinary , @z varbinary(32)
	SET @x = 1
	SET @z = 0x
	WHILE @x <=  (SELECT Max(Byte) FROM [config].[ObjectPolicyConfig])
		BEGIN
			SET @y = 0x + (SELECT Coalesce ( Convert (binary(1) , Sum(BitValue)),0) FROM [config].[ObjectPolicyConfig] WHERE Byte = @x AND Temp = 1)
			SET @z = @z + @y
		IF @x = (SELECT Max(Byte) FROM [config].[ObjectPolicyConfig])
			BREAK
		SET @x = @x + 1
		CONTINUE
	END
	RETURN @z
END


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetPolicyMaskValue] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[GetPolicyMaskValue] TO [OMSAdminRole]
    AS [dbo];

