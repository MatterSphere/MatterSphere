IF EXISTS (SELECT 1 FROM [dbo].[dbCommandCentreType] WHERE typeXML LIKE '%"NAVGRPDICTATION"%')
BEGIN
DECLARE @xml XML, @typeCode uCodeLookup

DECLARE db_cursor CURSOR FOR
SELECT typeCode, typeXML
FROM [dbo].[dbCommandCentreType]
WHERE typeXML LIKE '%"NAVGRPDICTATION"%'

OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @typeCode, @xml 

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @xml.modify('delete /Config/Dialog/Tabs/Tab[@group="NAVGRPDICTATION"]') 

    UPDATE [dbo].[dbCommandCentreType]
    SET typeXML = cast(@xml as nvarchar(max))
    WHERE typeCode = @typeCode 

    FETCH NEXT FROM db_cursor INTO @typeCode, @xml
END  

CLOSE db_cursor  
DEALLOCATE db_cursor

END
GO

DELETE FROM [dbo].[dbAdminMenu]
WHERE admnuCode IN ('AMUDICPOOL', 'AMUDICTATION')
GO
