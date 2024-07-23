
/*
      Procedure Version: 1
*/

CREATE PROCEDURE  [dbo].[sprGetSpecificData](@branch INT,@code NVARCHAR(30))
AS

if exists ( select spData from  dbspecificdata where splookup = @CODE and brid = @BRANCH ) 
  begin
	select spData from  dbspecificdata where splookup = @CODE and brid = @BRANCH 
   end
else
  begin
	select spData from  dbspecificdata where splookup = @CODE and brid =0
  end 


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetSpecificData] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetSpecificData] TO [OMSAdminRole]
    AS [dbo];

