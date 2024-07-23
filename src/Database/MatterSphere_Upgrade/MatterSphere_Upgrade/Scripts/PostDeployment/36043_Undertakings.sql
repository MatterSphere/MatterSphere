-- Hide cancel/save buttons on Undertakings tab in Command Center
DECLARE @xml XML;

DECLARE @T TABLE (rn INT PRIMARY KEY, lookup NVARCHAR(100), hidebuttons NVARCHAR(5))

DECLARE @rn INT
	, @lookup NVARCHAR(100)
	, @hidebuttons NVARCHAR(5)
	, @updated BIT = 0

SET @xml = (SELECT typeXML  FROM dbo.dbCommandCentreType WHERE typeCode = 'STANDARD')

INSERT INTO @T (rn, lookup, hidebuttons)
SELECT  ROW_NUMBER() OVER ( ORDER BY node ) AS rn,
    node.value('@lookup', 'NVARCHAR(100)') AS lookup,
    node.value('@hidebuttons', 'NVARCHAR(5)') AS hidebuttons
FROM    @xml.nodes('/Config/Dialog/Tabs/Tab') x ( node )


SELECT @rn = rn, @hidebuttons = hidebuttons FROM @T WHERE lookup = 'CLUNDERTAKINGS'

IF @rn IS NOT NULL
	IF @hidebuttons IS NULL
	BEGIN
		SET @xml.modify('insert attribute hidebuttons {"True"} into (/Config/Dialog/Tabs/Tab[sql:variable("@rn")])[1]')
		SET @updated = 1
	END

IF @updated = 1
	UPDATE dbo.dbCommandCentreType
		SET typeXML = CAST(@xml AS NVARCHAR(MAX))
	WHERE typeCode = 'STANDARD'