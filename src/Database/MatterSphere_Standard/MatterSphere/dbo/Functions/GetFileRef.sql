

CREATE FUNCTION [dbo].[GetFileRef](@clientno nvarchar(20), @fileno nvarchar(20))
RETURNS nvarchar(50) AS  
BEGIN 
return coalesce(@clientno, '') + (case when @fileno = null then '' else '/' end) + coalesce(@fileno, '')
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFileRef] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFileRef] TO [OMSAdminRole]
    AS [dbo];

