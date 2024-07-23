CREATE FUNCTION dbo.SplitString (
	@String NVARCHAR(MAX) = NULL
	, @Delim CHAR(1) = NULL
)
RETURNS @Values TABLE (
	Ordinal INT NOT NULL
	, Position INT NOT NULL
	, Value NVARCHAR(MAX) NOT NULL
	, [Count] INT NOT NULL
)
AS
BEGIN
	IF LEN(@String) > 0 AND @Delim IS NOT NULL
	BEGIN
		SET @String = CASE WHEN RIGHT(@String,1) = @Delim THEN @String ELSE @String + @Delim END;
		WITH StringTable AS (
			SELECT CHARINDEX(@Delim,@String) + 1 AS Position
				, LEFT(@String,CHARINDEX(@Delim, @String)-1) AS Value
				, 1 AS Ordinal
			
			UNION ALL
			
			SELECT CHARINDEX(@Delim, @String, Position) + 1 AS Position
				, SUBSTRING(@String, Position, CHARINDEX(@Delim, @String, Position) - Position) AS Value
				, Ordinal + 1 AS Ordinal
			FROM StringTable
			WHERE Position > 0
				AND Position <= LEN(@String)
		)
		INSERT INTO @Values (
			Ordinal
			, Position
			, Value
			, Count
		)
		SELECT Ordinal
			, Position - 1
			, Value
			, MAX(Ordinal) OVER () AS [Count]
		FROM StringTable
		OPTION (MAXRECURSION 0);
	END;
	RETURN;
END;
