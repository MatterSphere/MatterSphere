

CREATE VIEW [external].[vwdbClient]

AS
      SELECT  Entity.*
	  FROM [config].[dbClient] Entity where EXISTS
      (SELECT null 
      FROM         
      config.dbClient AS Clients LEFT JOIN 
      [external].[dbfile] Files on Files.clID = Clients.clID LEFT JOIN 
      [external].[ClientAccess]() Access on Access.clientID = Clients.clID LEFT JOIN
      [external].ClientAccessDenies Denies on Files.clID = Denies.ID
      WHERE
      (Access.clientID is not null
      or (Files.clID is not null AND Denies.ID is null)) AND Clients.clid = Entity.clID)
	  --AND Entity.SecurityOptions & 2 = 2    



GO
GRANT UPDATE
    ON OBJECT::[external].[vwdbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbClient] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbClient] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[vwdbClient] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[vwdbClient] TO [OMSApplicationRole]
    AS [dbo];

