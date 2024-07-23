Print 'Starting V5ScriptChanges\DBApi_Data.sql'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS ( SELECT apiGuid FROM dbo.dbAPI WHERE apiGuid = 'A3840119-FEB9-4980-96F2-B61E642D4A30' )
BEGIN
INSERT INTO [dbo].[dbAPI]([apiGUID],[apiCode],[apiDesc],[apiAuthor],[apiUIType],[apiDesigner],[apiRegistered],[apiService])
	 VALUES   ('A3840119-FEB9-4980-96F2-B61E642D4A30','omsexternalreports','Reports External','FWBS Ltd',1, 0, 1, 0)
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO