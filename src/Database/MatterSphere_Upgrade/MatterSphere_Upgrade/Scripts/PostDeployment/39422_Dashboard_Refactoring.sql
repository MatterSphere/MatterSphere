-- uXML dependent tables and procedures should be added via post-scripts. See 36179 Work Order for reference.

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 


IF NOT EXISTS(SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'dbo.dbDashboards') )
BEGIN
    CREATE TABLE [dbo].[dbDashboards]
    (
        [dshObjCode] [dbo].[uCodeLookup] NOT NULL,
        [dshSystem] BIT NOT NULL,
        [dshConfig] [dbo].[uXML] NOT NULL,
        [dshActive] BIT NOT NULL DEFAULT 1,
        [dshTypeCompatible] TINYINT NOT NULL DEFAULT 0,
        [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbDashboards_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
        CONSTRAINT [PK_dbDashboards] PRIMARY KEY CLUSTERED ([dshObjCode] ASC) WITH (FILLFACTOR = 90)
    )

    CREATE UNIQUE NONCLUSTERED INDEX [IX_dbDashboards_rowguid]
        ON [dbo].[dbDashboards]([rowguid] ASC) WITH (FILLFACTOR = 90)
        ON [IndexGroup];

    GRANT UPDATE
        ON OBJECT::[dbo].[dbDashboards] TO [OMSApplicationRole]
        AS [dbo];

    GRANT SELECT
        ON OBJECT::[dbo].[dbDashboards] TO [OMSApplicationRole]
        AS [dbo];

    GRANT INSERT
        ON OBJECT::[dbo].[dbDashboards] TO [OMSApplicationRole]
        AS [dbo];

    GRANT DELETE
        ON OBJECT::[dbo].[dbDashboards] TO [OMSApplicationRole]
        AS [dbo];

    INSERT INTO [dbo].[dbDashboards]
       ([dshObjCode]
       ,[dshSystem]
       ,[dshConfig])
    VALUES
       ('DSHSYSTEM'
       ,1
       ,'<config></config>')

        DECLARE @Table Table (cmdXml XML, typeCode nvarchar(15))
        INSERT INTO @Table SELECT CAST(typeXML AS XML) as cmdXml, typeCode FROM dbo.dbCommandCentreType WHERE typeCode <> 'SUPPORT'
        UPDATE @Table SET cmdXml.modify('insert <Tab lookup="DASHBOARD" source="DSHSYSTEM" tabtype="Addin" group="CMNDCTRCAPTION" hidebuttons="True" glyph="38" conditional="Ispackageinstalled(&quot;CommandCentre&quot;)" /> as first into (/Config/Dialog/Tabs)[1]')

        UPDATE dbo.dbCommandCentreType 
        SET typeXML = CAST(t.cmdXml as nvarchar(max)) 
        FROM dbo.dbCommandCentreType cc 
        INNER JOIN @Table t ON t.typeCode = cc.typeCode
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[dbDashboards]') AND [name] = 'dshActive')
    BEGIN
        ALTER TABLE [dbo].[dbDashboards] ADD [dshActive] [bit] NOT NULL DEFAULT 1
    END
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[dbDashboards]') AND [name] = 'dshTypeCompatible')
    BEGIN
        ALTER TABLE [dbo].[dbDashboards] ADD [dshTypeCompatible] [TINYINT] NOT NULL DEFAULT 0
    END
END

GO
IF NOT EXISTS(SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'dbo.dbUserDashboards') )
BEGIN
    CREATE TABLE [dbo].[dbUserDashboards]
    (
        [usrID] [int] NOT NULL,
        [dshObjCode] [dbo].[uCodeLookup] NOT NULL, 
        [dshConfig] [dbo].[uXML] NOT NULL, 
        [rowguid] UNIQUEIDENTIFIER CONSTRAINT [DF_dbUserDashboards_rowguid] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
        CONSTRAINT [PK_dbUserDashboards] PRIMARY KEY CLUSTERED ([usrID], [dshObjCode] ASC) WITH (FILLFACTOR = 90)
    )

    CREATE UNIQUE NONCLUSTERED INDEX [IX_dbUserDashboards_rowguid]
        ON [dbo].[dbUserDashboards]([rowguid] ASC) WITH (FILLFACTOR = 90)
        ON [IndexGroup];

    GRANT UPDATE
        ON OBJECT::[dbo].[dbUserDashboards] TO [OMSApplicationRole]
        AS [dbo];

    GRANT SELECT
        ON OBJECT::[dbo].[dbUserDashboards] TO [OMSApplicationRole]
        AS [dbo];

    GRANT INSERT
        ON OBJECT::[dbo].[dbUserDashboards] TO [OMSApplicationRole]
        AS [dbo];

    GRANT DELETE
        ON OBJECT::[dbo].[dbUserDashboards] TO [OMSApplicationRole]
        AS [dbo];
END

GO
IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID(N'dbo.sprAddDeleteUserDashboardCells'))
    DROP PROCEDURE dbo.sprAddDeleteUserDashboardCells

GO
IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID(N'dbo.sprUpdateUserDashboards'))
    DROP PROCEDURE dbo.sprUpdateUserDashboards

GO
CREATE PROCEDURE dbo.sprUpdateUserDashboards
    @usrID INT
    ,@dshObjCode uCodeLookup
    ,@dshConfig dbo.uXML
    ,@isConfigurationMode BIT
AS SET NOCOUNT ON
    IF @isConfigurationMode = 1
        UPDATE dbo.dbDashboards SET dshConfig = @dshConfig WHERE dshObjCode = @dshObjCode
    ELSE
        IF EXISTS(SELECT 1 FROM dbo.dbUserDashboards WITH(NOLOCK) WHERE usrID = @usrID AND dshObjCode = @dshObjCode)
            IF ISNULL(@dshConfig, '') = ''
                DELETE dbo.dbUserDashboards WHERE usrID = @usrID AND dshObjCode = @dshObjCode
            ELSE 
                UPDATE dbo.dbUserDashboards SET dshConfig = @dshConfig WHERE usrID = @usrID AND dshObjCode = @dshObjCode
        ELSE
            IF ISNULL(@dshConfig, '') <> ''
                INSERT INTO dbo.dbUserDashboards([usrID],[dshObjCode],[dshConfig])
                VALUES(@usrID, @dshObjCode, @dshConfig)

GO
GRANT EXECUTE
    ON OBJECT::dbo.sprUpdateUserDashboards TO [OMSRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::dbo.sprUpdateUserDashboards TO [OMSAdminRole]
    AS [dbo];

GO
IF EXISTS(SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'dbo.dbDashboardCells'))
BEGIN
    DECLARE @ColumnsConfigTable Table(usrID int, dshbType nvarchar(15), objCode nvarchar(15), code nvarchar(15), name nvarchar(15), visible nvarchar(15))
    DECLARE @ColumnsXMLTable Table(usrID int, dshbType nvarchar(15), objCode nvarchar(15), config XML)

    INSERT INTO @ColumnsXMLTable SELECT usrID, dshbType, objCode, CAST(columnsSettings as XML) from dbo.dbDashboardCells

    INSERT INTO @ColumnsConfigTable SELECT usrID, dshbType, objCode
        ,pageNodeColumn.value('@Code', 'nvarchar(15)') AS code
        ,columnNodecolumn.value('@Name[1]', 'nvarchar(15)') AS name
        ,columnNodecolumn.value('@Visible[1]', 'nvarchar(15)') AS visible
    FROM @ColumnsXMLTable cxml
    CROSS APPLY cxml.config.nodes('/ColumnsSettings/Page') as XMLtable1(pageNodeColumn)
    CROSS APPLY XMLtable1.pageNodeColumn.nodes('Column') as XMLtable2(columnNodecolumn)

    DECLARE @Table TABLE (usrId BIGINT, config uXML)
        INSERT INTO @Table
            SELECT usrID , 
            '<config>' +
                (SELECT posRow AS '@row', posColumn AS '@column', objCode AS '@omsObjCode', dshbType AS '@dshbType', LOWER(NEWID()) AS '@guid',
                    (SELECT code as '@code',
                        ((SELECT name AS '@name', visible AS '@visible'
                        FROM @ColumnsConfigTable cct1 
                        WHERE cct1.code = cct2.code 
                        GROUP BY code, name, visible
                        FOR XML PATH ('column'), TYPE)
                        ) AS 'columns'
                    FROM @ColumnsConfigTable cct2
                    WHERE cct2.usrID = dc2.usrID AND (cct2.dshbType = dc1.dshbType OR cct2.objCode = dc1.objCode)
                    GROUP BY code, usrId, dshbType, objCode
                    FOR XML PATH('page'), TYPE, ROOT('pages'))
                FROM dbDashboardCells dc1 
                WHERE dc1.usrID = dc2.usrID 
                GROUP BY posRow, posColumn, objCode,dshbType, columnsSettings 
                FOR XML PATH ('tile'), ROOT ('tiles')) + '</config>' AS config 
            FROM dbDashboardCells dc2 
            GROUP BY usrID

    DECLARE @UserId BIGINT = (SELECT MIN(usrId) FROM @Table) - 1
    DECLARE @Config uXML
    WHILE (1 = 1)
    BEGIN
        SELECT TOP 1 @UserId = usrId, @Config = config FROM @Table WHERE usrId > @UserId ORDER BY usrId
        IF @@ROWCOUNT = 0 BREAK;
        EXEC dbo.sprUpdateUserDashboards @UserId, 'DSHSYSTEM', @Config , 0
    END

    DROP TABLE dbo.dbDashboardCells

END

GO
IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbOMSObjects' AND COLUMN_NAME = 'DashboardParams')
BEGIN
    DECLARE @Sql nvarchar(max) = 
    N'DECLARE @DashboardParamsXMLTable Table(ObjCode nvarchar(15), DashboardParams XML)
    DECLARE @ConvertedDashboardParamsTable Table(ObjCode nvarchar(15), DashboardParams nvarchar(max))

    INSERT INTO @DashboardParamsXMLTable SELECT ObjCode, DashboardParams FROM [dbo].[dbOMSObjects] WHERE [DashboardParams] LIKE ''%Size%''

    INSERT INTO @ConvertedDashboardParamsTable
        SELECT ObjCode
        ,''<config><params code="'' + pageNodeColumn.value(''@Code'', ''nvarchar(15)'') + ''" width="''
        + pageNodeColumn.value(''@Size'', ''nvarchar(15)'') + ''" height="1" priority="'' + pageNodeColumn.value(''@Priority'', ''nvarchar(15)'') + ''"''
        + COALESCE('' user_roles="'' + pageNodeColumn.value(''@userRoles'', ''nvarchar(max)'') +''"'', '''')+'' /></config>''
    FROM @DashboardParamsXMLTable cxml
    CROSS APPLY DashboardParams.nodes(''/Cell'') as t(pageNodeColumn)

    UPDATE [dbo].[dbOMSObjects]
        SET TileParams = c.DashboardParams
    FROM [dbo].[dbOMSObjects] o
    INNER JOIN @ConvertedDashboardParamsTable c ON o.ObjCode = c.ObjCode

    ALTER TABLE [dbo].[dbOMSObjects] DROP COLUMN [DashboardParams]'
    EXEC sp_executesql @Sql
END


