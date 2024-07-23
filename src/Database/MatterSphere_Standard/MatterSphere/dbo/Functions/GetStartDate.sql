

CREATE FUNCTION [dbo].[GetStartDate] (@d datetime)  
RETURNS datetime AS  
BEGIN 
return convert(datetime, (convert(nvarchar, @d, 101)))
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStartDate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStartDate] TO [OMSAdminRole]
    AS [dbo];

