

CREATE FUNCTION [dbo].[GetEndDate] (@d datetime)  
RETURNS datetime AS  
BEGIN 
set @d = dateadd(d, 1, @d)
return convert(datetime, (convert(nvarchar, @d, 101)))
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetEndDate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetEndDate] TO [OMSAdminRole]
    AS [dbo];

