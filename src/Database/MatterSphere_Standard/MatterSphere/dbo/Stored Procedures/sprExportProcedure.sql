

CREATE PROCEDURE [dbo].[sprExportProcedure] (@name nvarchar(100))
as
select [text] as definition from syscomments where id = object_id(@name)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprExportProcedure] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprExportProcedure] TO [OMSAdminRole]
    AS [dbo];

