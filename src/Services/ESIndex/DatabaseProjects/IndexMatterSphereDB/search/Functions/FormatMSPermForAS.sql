CREATE FUNCTION [search].[FormatMSPermForAS] ( @Type as nvarchar(10),@CTV as search.ESUGPD readonly)
RETURNS @TR table (ID BIGINT, UGDP nvarchar(max),PRIMARY KEY (ID)) 
AS

BEGIN

DECLARE @T table (ID BIGINT, rn BIGINT, UGDP nvarchar(max), PRIMARY KEY (ID, rn))

DECLARE @maxnum as bigint

 If not exists (select 1 from @CTV)
 begin
	If @Type = 'Client'
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbClient' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				RUGC.[ClientID],
				row_number() over (PARTITION BY RUGC.[ClientID] order by RUGC.[ClientID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGC.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 5 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 5 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END
		FROM
				[relationship].[UserGroup_Client] RUGC
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
					JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID
				ON RUGC.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGC.[UserGroupID] = G.[ID]
		end
	END

  	If @Type = 'Contact' 
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbContact' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				RUGC.[ContactID],
				row_number() over (PARTITION BY RUGC.[ContactID] order by RUGC.[ContactID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGC.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 10 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 10 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END
		FROM
				[relationship].[UserGroup_Contact] RUGC
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
					JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID			
				ON RUGC.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGC.[UserGroupID] = G.[ID]
		end
	END

	If @Type = 'Associate' 
	/****** LINK Associate to Contact permissions *******/
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbContact' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				A.[AssocID],
				row_number() over (PARTITION BY A.[AssocID] order by A.[AssocID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGC.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 10 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 10 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END
		FROM
				[relationship].[UserGroup_Contact] RUGC
		JOIN 
				[config].[dbAssociates] A on A.contid = RUGC.Contactid
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
					JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID			
				ON RUGC.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGC.[UserGroupID] = G.[ID]
		end
	END

 	If @Type = 'File'
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbFile' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				RUGF.[FileID],
				row_number() over (PARTITION BY RUGF.[FileID] order by RUGF.[FileID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGF.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 6 , 1 ) & 32 = 32 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 6 , 1 ) & 32 = 32 THEN '(DENY)' ELSE '(ALLOW)' END END

		FROM
				[relationship].[UserGroup_File] RUGF
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
					JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID
				ON RUGF.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGF.[UserGroupID] = G.[ID]
		end
	END


	If @Type = 'Document' OR @Type = 'Email'
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbDocument' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				RUGD.[DocumentID],
				row_number() over (PARTITION BY RUGD.[DocumentID] order by RUGD.[DocumentID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGD.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END
		FROM
				[relationship].[UserGroup_Document] RUGD
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
					JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID		
				ON RUGD.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGD.[UserGroupID] = G.[ID]

		IF exists (select 1 from dbRegInfo where regBlockInheritence = 1)
		begin
			
			SET @maxnum = ISNULL((SELECT MAX(rn) FROM @T), 0)
			
			INSERT INTO @T 
			SELECT 
					D.DocID,
					row_number() over (PARTITION BY D.DocID order by D.DocID ) + @maxnum as rn,
					coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGF.[UserGroupID] as nvarchar(40)))+
					CASE WHEN Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
					CASE WHEN Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END

			FROM
					[relationship].[UserGroup_File] RUGF
			JOIN
					[config].[dbdocument] D ON D.fileID = RUGF.FileID
			JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
			LEFT JOIN
					[item].[User]  U 
						JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID		
					ON RUGF.[UserGroupID] = U.[ID]
			LEFT JOIN
					[item].[Group]  G ON RUGF.[UserGroupID] = G.[ID]
		end
	  end
	end
end
else
begin

  	If @Type = 'Client'
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbClient' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				RUGC.[ClientID],
				row_number() over (PARTITION BY RUGC.[ClientID] order by RUGC.[ClientID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGC.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 5 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 5 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END
		FROM	@CTV AS [CT]
		JOIN
				[relationship].[UserGroup_Client] RUGC ON CONVERT(nvarchar(38),RUGC.ClientID) = CT.[InsertedID]
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
						JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID		
				ON RUGC.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGC.[UserGroupID] = G.[ID]
		end
	END

  	If @Type = 'Contact' 
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbContact' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				RUGC.[ContactID],
				row_number() over (PARTITION BY RUGC.[ContactID] order by RUGC.[ContactID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGC.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 10 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 10 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END
		FROM	@CTV AS [CT]
		JOIN
				[relationship].[UserGroup_Contact] RUGC ON CONVERT(nvarchar(38),RUGC.contactid) = CT.[InsertedID]
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
						JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID		
				ON RUGC.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGC.[UserGroupID] = G.[ID]
		end
	END

		/****** LINK Associate to Contact permissions *******/
  	If @Type = 'Associate' 
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbContact' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				A.[AssocID],
				row_number() over (PARTITION BY A.[AssocID] order by A.[AssocID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGC.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 10 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 10 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END
		FROM	@CTV AS [CT]
		JOIN
				[config].[dbAssociates] A on A.AssocID = CT.[InsertedID]
		JOIN
				[relationship].[UserGroup_Contact] RUGC ON CONVERT(nvarchar(38),RUGC.contactid) = A.contid	
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
						JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID		
				ON RUGC.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGC.[UserGroupID] = G.[ID]
		end
	END

 	If @Type = 'File'
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbFile' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				RUGF.[FileID],
				row_number() over (PARTITION BY RUGF.[FileID] order by RUGF.[FileID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGF.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 6 , 1 ) & 32 = 32 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 6 , 1 ) & 32 = 32 THEN '(DENY)' ELSE '(ALLOW)' END END

		FROM	@CTV AS [CT]
		JOIN
				[relationship].[UserGroup_File] RUGF on CONVERT(nvarchar(38),RUGF.Fileid) = CT.[InsertedID]
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
						JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID		
				ON RUGF.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGF.[UserGroupID] = G.[ID]
		end
	END


	If @Type = 'Document' OR @Type = 'Email'
	BEGIN
	 IF exists (select 1 from sys.tables where name = 'dbDocument' and [Schema_id] = SCHEMA_ID('Config'))
	 begin
		INSERT INTO @T 
		SELECT 
				RUGD.[DocumentID],
				row_number() over (PARTITION BY RUGD.[DocumentID] order by RUGD.[DocumentID] ) as rn,
				coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGD.[UserGroupID] as nvarchar(40)))+
				CASE WHEN Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
				CASE WHEN Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END
		FROM	@CTV AS [CT]
		JOIN
				[relationship].[UserGroup_Document] RUGD ON CONVERT(nvarchar(38),RUGD.DocumentID) = CT.[InsertedID]
		JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
		LEFT JOIN
				[item].[User]  U 
						JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID		
				ON RUGD.[UserGroupID] = U.[ID]
		LEFT JOIN
				[item].[Group]  G ON RUGD.[UserGroupID] = G.[ID]

		IF exists (select 1 from dbRegInfo where regBlockInheritence = 1)
		begin
			SET @maxnum = ISNULL((SELECT MAX(rn) FROM @T), 0)
			
			INSERT INTO @T 
			SELECT 
					D.DocID,
					row_number() over (PARTITION BY D.DocID order by D.DocID ) + @maxnum as rn,
					coalesce(CAST(U.ID AS NVARCHAR(40)),CAST(RUGF.[UserGroupID] as nvarchar(40)))+
					CASE WHEN Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128 THEN '(ALLOW)' ELSE 
					CASE WHEN Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128 THEN '(DENY)' ELSE '(ALLOW)' END END

			FROM	@CTV AS [CT]
			JOIN
					[config].[dbdocument] D ON CONVERT(nvarchar(38),D.DocID) = CT.[InsertedID]
			JOIN
					[relationship].[UserGroup_File] RUGF ON D.fileID = RUGF.FileID
			JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
			LEFT JOIN
					[item].[User]  U 
						JOIN [dbo].[DBUser] du on u.NTLogin = du.usrADID		
					ON RUGF.[UserGroupID] = U.[ID]
			LEFT JOIN
					[item].[Group]  G ON RUGF.[UserGroupID] = G.[ID]
		end
	  end
	end
end;

WITH TR AS(
SELECT distinct
      ID
      FROM @T
)
INSERT INTO @TR
SELECT TR.ID,
STUFF((
			SELECT ' ' + T.UGDP
			FROM @T as T 
			WHERE T.ID = TR.ID 
			ORDER BY rn
			FOR XML PATH(''), TYPE).value('(./text())[1]','NVARCHAR(MAX)'), 1, 1, '')
FROM TR 

RETURN

END