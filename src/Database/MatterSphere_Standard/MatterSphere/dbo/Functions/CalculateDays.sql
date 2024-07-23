

CREATE FUNCTION [dbo].[CalculateDays] (@d1 datetime, @d2 datetime)  
RETURNS int 
AS  
BEGIN 

 return Ceiling(DateDiff(n,@d1, @d2)/1440.00)
 
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CalculateDays] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CalculateDays] TO [OMSAdminRole]
    AS [dbo];

