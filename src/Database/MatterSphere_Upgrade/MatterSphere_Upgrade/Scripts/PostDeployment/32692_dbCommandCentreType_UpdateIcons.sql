DECLARE @xml XML;

DECLARE @T TABLE (rn INT PRIMARY KEY, source NVARCHAR(100), glyph NVARCHAR(5))
DECLARE @TMap TABLE (source NVARCHAR(100) PRIMARY KEY, glyph NVARCHAR(5))

INSERT INTO @TMap (source, glyph)
VALUES ('SCHFEEFILELIST', '18')
	, ('SCHFEEREVMGR', '26')
	, ('ADDFMTASKSUSR', '4')
	, ('SCHFEEMSMANAGER', '13')
	, ('SCHFEEAPPLST', '3')
	, ('SCHCLIARCHALL', '8')
	, ('SCHCOMFINAUTH', '15')
	, ('SCHCOMAWAITACK', '25')
	, ('SCHFEERSKMANAGE', '20')
	, ('UDSCHPROOFOFID', '19')
	, ('SCHCOMSCANPOST', '1')
	, ('SCRCOMWEBFAVS', '7')
	, ('SCHCOMCOMPMNG', '17')
	, ('SCHCOMUNDTAKING', '24')

DECLARE @rn INT
	, @source NVARCHAR(100)
	, @glyph NVARCHAR(5)
	, @updated BIT = 0

SET @xml = (SELECT typeXML  FROM dbo.dbCommandCentreType WHERE typeCode = 'STANDARD')

INSERT INTO @T (rn, source, glyph)
SELECT  ROW_NUMBER() OVER ( ORDER BY node ) AS rn,
    node.value('@source', 'NVARCHAR(100)') AS source,
    node.value('@glyph', 'NVARCHAR(5)') AS glyph
FROM    @xml.nodes('/Config/Dialog/Tabs/Tab') x ( node )

DELETE t
FROM @T t
WHERE NOT EXISTS(SELECT 1 FROM @TMap tm WHERE tm.source = t.source)

DELETE t
FROM @T t
WHERE EXISTS(SELECT 1 FROM @TMap tm WHERE tm.source = t.source AND tm.glyph = t.glyph)

SET @rn = (SELECT TOP 1 rn FROM @T ORDER BY rn)

WHILE @rn IS NOT NULL
BEGIN
	SET @source = (SELECT source FROM @T WHERE rn = @rn)
	SET @glyph = (SELECT glyph FROM @TMap WHERE source = @source)

	SET @xml.modify('replace value of (/Config/Dialog/Tabs/Tab[sql:variable("@rn")]/@glyph)[1] with sql:variable("@glyph")')
	
	SET @rn = (SELECT TOP 1 rn FROM @T WHERE rn > @rn ORDER BY rn)
	
	SET @updated = 1
END

IF @updated = 1
	UPDATE dbo.dbCommandCentreType 
		SET typeXML = CAST(@xml AS NVARCHAR(MAX))
	WHERE typeCode = 'STANDARD'

DELETE @T
DELETE @TMap

INSERT INTO @TMap (source, glyph)
VALUES ('GRPSUPUI', '0')
	, ('GRPSUPEXTDATA', '28')
	, ('GRPSUPPKG', '8')
	, ('SCRSQLCONFIG', '28')
	, ('SCRSQLFILES', '28')
	, ('SCHSERVERPRO', '28')
	, ('SCHSERVERCHKSUM', '28')


SET @xml = (SELECT typeXML  FROM dbo.dbCommandCentreType WHERE typeCode = 'SUPPORT')

INSERT INTO @T (rn, source, glyph)
SELECT  ROW_NUMBER() OVER ( ORDER BY node ) AS rn,
    node.value('@source', 'NVARCHAR(100)') AS source,
    node.value('@glyph', 'NVARCHAR(5)') AS glyph
FROM    @xml.nodes('/Config/Dialog/Tabs/Tab') x ( node )

DELETE t
FROM @T t
WHERE NOT EXISTS(SELECT 1 FROM @TMap tm WHERE tm.source = t.source)

DELETE t
FROM @T t
WHERE EXISTS(SELECT 1 FROM @TMap tm WHERE tm.source = t.source AND tm.glyph = t.glyph)

SET @rn = (SELECT TOP 1 rn FROM @T ORDER BY rn)

WHILE @rn IS NOT NULL
BEGIN
	SET @source = (SELECT source FROM @T WHERE rn = @rn)
	SET @glyph = (SELECT glyph FROM @TMap WHERE source = @source)

	SET @xml.modify('replace value of (/Config/Dialog/Tabs/Tab[sql:variable("@rn")]/@glyph)[1] with sql:variable("@glyph")')
	
	SET @rn = (SELECT TOP 1 rn FROM @T WHERE rn > @rn ORDER BY rn)
	
	SET @updated = 1
END

IF @updated = 1
	UPDATE dbo.dbCommandCentreType 
		SET typeXML = CAST(@xml AS NVARCHAR(MAX))
	WHERE typeCode = 'SUPPORT'
