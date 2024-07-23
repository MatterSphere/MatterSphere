

CREATE PROCEDURE [config].[UpdatePolicy] 
	@maskString nvarchar(2000) ,
	@type uCodeLookup , 
	@policyID uniqueidentifier
	
AS
SET NOCOUNT ON
--declare @maskString nvarchar(2000) , @policy uniqueidentifier
--set @maskstring = '|2,1,0,0|2,2,0,0|3,1,0,0|4,2,1,0|4,4,0,0|4,8,1,0|4,16,1,0|6,2,1,0|6,4,0,0|6,8,1,0|6,16,1,0|8,2,0,0|8,4,1,0|8,8,1,0|8,16,1,0|'

--DECLARE @maskTable table
--(
--	[Byte] tinyint,
--	[Bit] tinyint,
--	[Allow] bit,
--	[Deny] bit
--)

IF ( SELECT [type] FROM [config].[ObjectPolicy] WHERE [ID] = @policyID ) <> 'CUSTOM'
BEGIN
	-- If the policy is not a custom policy don't do anything just return as you cannot edit any other policy except custom
	RETURN
END


DECLARE  @x smallint , @y varchar(20) , @z smallint , @stringLen smallint , @SQL varchar(2000)
CREATE TABLE #maskTable
(
	[Byte] tinyint,
	[BitValue] int,
	[Allow] bit,
	[Deny] bit
)
SET @z  = 1
SET @stringLen = 1
--IF @name = 'CUSTOM'
--BEGIN
--	RAISERROR ('Cannot create template policy CUSTOM name is reserved', 16 , 1)
--	RETURN
--END

WHILE @stringLen > 0
BEGIN
	SET @x = @z + CharIndex ( '|' , @maskString  ) 
	SET @z = CharIndex ('|' , @maskString , @x)
	SET @stringLen = LEN (@maskString) - @z
	SET @y = Substring ( @maskString , @x , @z - @x)
	SET @SQL = ' INSERT #maskTable VALUES ( ' + @y + ')'
	EXEC (@SQL)
END

DECLARE @x1 tinyint , @y1 varbinary , @allowMask varbinary(16) , @x2 tinyint , @y2 varbinary , @denyMask varbinary(16)
SET @x1 = 1
SET @allowMask = 0x
SET @denyMask = 0x
WHILE @x1 <=  16
BEGIN
SET @y1 = 0x + (SELECT Coalesce ( Convert (binary(1) , Sum(S.BitValue)),0) FROM
	(
	SELECT
		PC.byte,
		PC.bitValue,
		M.[allow],
		M.[deny]
	FROM
		[config].[ObjectPolicyConfig] PC
	LEFT JOIN
		#masktable	m ON PC.byte = M.byte and PC.bitvalue = M.[bitvalue]
	WHERE 
		PC.Byte = @x1 AND M.Allow = 1
	) S )
SET @y2 = 0x + (SELECT Coalesce ( Convert (binary(1) , Sum(S1.BitValue)),0) FROM
	(
	SELECT
		PC.byte,
		PC.bitValue,
		M.[allow],
		M.[deny]
	FROM
		[config].[ObjectPolicyConfig] PC
	LEFT JOIN
		#masktable	m ON PC.byte = M.byte and PC.bitvalue = M.[bitvalue]
	WHERE 
		PC.Byte = @x1 AND M.[Deny] = 1
	) S1 )
SET @allowMask = @allowMask + @y1
SET @denyMask = @denyMask + @y2
IF @x1 = 16
			BREAK
		SET @x1 = @x1 + 1
		CONTINUE
	END


drop table #masktable

UPDATE [config].[ObjectPolicy]
SET allowMask = @allowMask ,DenyMask = @denyMask 
WHERE ID = @policyID



GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdatePolicy] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[UpdatePolicy] TO [OMSAdminRole]
    AS [dbo];

