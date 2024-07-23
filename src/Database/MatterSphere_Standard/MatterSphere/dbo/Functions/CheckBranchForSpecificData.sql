CREATE FUNCTION dbo.CheckBranchForSpecificData ()
RETURNS BIT
AS
BEGIN
       DECLARE @BRID INT 
       DECLARE @RET BIT 

       SELECT @BRID = BRID FROM DBSPECIFICDATA WHERE BRID != 0 AND BRID NOT IN ( SELECT BRID FROM DBBRANCH ) 
       IF @BRID != 0 
              SET @RET = 0
       ELSE
              SET @RET = 1

       RETURN @RET

 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckBranchForSpecificData] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckBranchForSpecificData] TO [OMSAdminRole]
    AS [dbo];

