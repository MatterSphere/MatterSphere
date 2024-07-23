

CREATE FUNCTION [dbo].[GetFileRefByID](@fileid bigint)
RETURNS nvarchar(50) AS  
BEGIN 
declare @clno nvarchar(20)
declare @fileno nvarchar(20)
select @clno = clno, @fileno = fileno from dbfile inner join dbclient on dbfile.clid = dbclient.clid where fileid = @fileid
return dbo.GetFileRef(@clno, @fileno)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFileRefByID] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFileRefByID] TO [OMSAdminRole]
    AS [dbo];

