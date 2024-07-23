Print 'Starting V5ScriptChanges\DBAPIFWBSUPdate.sql'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

update dbapi set apiconsumertype = 1 | (case apiservice when 1 then 4 else 0 end) | (case apidesigner when 1 then 2 else 0 end)
update dbapi set apiconsumertype = apiconsumertype | 8 where apiguid= '31ED0BE4-3B00-469F-B50B-7CF27154AF9C' 
update dbapi set apicompanyid = -190021717, apipublickeytoken = '7fb1051d3d2d9ebc' where apiauthor = 'FWBS Ltd'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO