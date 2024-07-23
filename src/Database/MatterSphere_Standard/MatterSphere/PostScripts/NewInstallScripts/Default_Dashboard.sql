Print 'Starting NewInstallScripts\Default_Dashboard.sql'

IF NOT EXISTS ( SELECT [dshObjCode] FROM [dbo].[dbDashboards] WHERE [dshObjCode] = 'DSHSYSTEM' )
BEGIN
    INSERT INTO [dbo].[dbDashboards]
        ([dshObjCode]
        ,[dshSystem]
        ,[dshConfig]
        ,[dshTypeCompatible])
    VALUES
        ('DSHSYSTEM'
        ,1
        ,'<config></config>'
        ,0)
END



