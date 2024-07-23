

CREATE PROCEDURE [dbo].[fwbsDataListXDtables]

AS
SELECT [name] as ID , [name]  FROM sysobjects  WHERE [type] = 'u'  and [name] like 'ud%'

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsDataListXDtables] TO [OMSAdminRole]
    AS [dbo];

