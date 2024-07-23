

CREATE FUNCTION [dbo].[GetCoreImportConfig] (@Type uCodeLookup, @Source uCodeLookup, @Filter1 nvarchar(100), @Filter2 nvarchar(100), @Filter3 nvarchar(100))  
RETURNS bigint AS  
BEGIN 
	declare @id bigint
	select @id = ciid from dbcoreimportconfig where citype = @Type and cisource = @source and coalesce(cifilter1, '') = coalesce(@filter1, '') and coalesce(cifilter2, '') = coalesce(@filter2, '') and coalesce(cifilter3, '') = coalesce(@filter3, '')
	if (@id is null)
		select @id = ciid from dbcoreimportconfig where citype = @Type and cisource is null and coalesce(cifilter1, '') = coalesce(@filter1, '') and coalesce(cifilter2, '') = coalesce(@filter2, '') and coalesce(cifilter3, '') = coalesce(@filter3, '')
	
	return @id
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCoreImportConfig] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCoreImportConfig] TO [OMSAdminRole]
    AS [dbo];

