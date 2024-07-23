

-- Returns document IDs that you are allowed to see, based on your security permissions

CREATE PROCEDURE [config].[ListAllowedDocIDs]
       @DOCIDs nvarchar(max)
AS
BEGIN     

       set nocount on;
          declare @USER nvarchar(100) 
          set @USER = config.GetUserLogin()

          select r.items  from dbo.SplitStringToTable(@DOCIDs, ',')  r 
                     where not exists (select 1 from [config].[CheckDocumentAccess] (@USER,items) d  where  d.[VDeny] = 1 or (d.[VDeny] is NULL and d.[VAllow] is NULL))

             
end



SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[config].[ListAllowedDocIDs] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[config].[ListAllowedDocIDs] TO [OMSAdminRole]
    AS [dbo];

