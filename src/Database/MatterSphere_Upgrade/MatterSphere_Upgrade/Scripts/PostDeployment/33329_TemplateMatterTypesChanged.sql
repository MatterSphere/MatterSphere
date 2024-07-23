DECLARE @xml XML
	, @rn INT
	, @updated BIT = 0
DECLARE @T TABLE (rn INT PRIMARY KEY, lookup NVARCHAR(MAX))

SET @xml = (SELECT CAST(typeXML AS XML) FROM dbo.dbFileType WHERE typeCode = 'TEMPLATE')

INSERT INTO @T (rn, lookup)
SELECT  ROW_NUMBER() OVER ( ORDER BY node ) AS rn,
    node.value('@lookup', 'NVARCHAR(100)') AS lookup
FROM @xml.nodes('/Config/Dialog/Panels/Panel') x ( node )

SET @rn = (SELECT rn FROM @T WHERE lookup = 'ACTIONPANEL')

IF @rn IS NOT NULL
BEGIN
	SET @xml.modify('delete /Config/Dialog/Panels/Panel[sql:variable("@rn")]')
	SET @updated = 1
END


SET @rn = (SELECT rn FROM @T WHERE lookup = 'CLDETAILS')
IF @rn IS NULL
BEGIN
	SET @updated = 1

	DELETE @T

	INSERT INTO @T (rn, lookup)
	SELECT  ROW_NUMBER() OVER ( ORDER BY node ) AS rn,
		node.value('@lookup', 'NVARCHAR(100)') AS lookup
	FROM @xml.nodes('/Config/Dialog/Panels/Panel') x ( node )

	SET @rn = (SELECT rn FROM @T WHERE lookup = '-1613643696')	
	IF @rn IS NOT NULL
		SET @xml.modify('insert <Panel lookup="CLDETAILS" property="FileClientDescription" /> after (/Config/Dialog/Panels/Panel[sql:variable("@rn")])[1]')
	ELSE
		SET @xml.modify('insert <Panel lookup="CLDETAILS" property="FileClientDescription" /> as first into (/Config/Dialog/Panels)[1]')
END


DELETE @T

INSERT INTO @T (rn, lookup)
SELECT  ROW_NUMBER() OVER ( ORDER BY node ) AS rn,
	node.value('@lookup', 'NVARCHAR(100)') AS lookup
FROM    @xml.nodes('/Config/Dialog/Tabs/Tab') x ( node )

SET @rn = (SELECT rn FROM @T WHERE lookup = 'CLIDETAILSTAB')

IF @rn IS NULL
BEGIN
	SET @rn = (SELECT rn FROM @T WHERE lookup = 'FILEDETAILS')

	IF @rn IS NOT NULL
		SET @xml.modify('insert <Tab lookup="CLIDETAILSTAB" order="1" source="SCRCLIDETAILS" tabtype="Enquiry" group="NAVGRPFILINFO" /> after (/Config/Dialog/Tabs/Tab[sql:variable("@rn")])[1]')
	ELSE
		SET @xml.modify('insert <Tab lookup="CLIDETAILSTAB" order="1" source="SCRCLIDETAILS" tabtype="Enquiry" group="NAVGRPFILINFO" /> as last into (/Config/Dialog/Tabs)[1]')

	SET @updated = 1
END

IF @updated = 1
	UPDATE dbo.dbFileType
		SET typeXML = CAST(@xml AS NVARCHAR(MAX))
	WHERE typeCode = 'TEMPLATE'
