	CREATE FUNCTION [config].[searchDocumentAccess] (@type nvarchar(10),@id bigint )
RETURNS @T table (DocumentID BIGINT,Allow bit,[Deny] bit,secure bit,clid bigint,fileid bigint)
AS
begin
	DECLARE @USER nvarchar(200)
	SET @USER = config.GetUserLogin() 
	DECLARE @Sec INT
	select @SEC = [config].[IsAdministrator] (@User)

		if NOT exists (SELECT 1  FROM [dbo].[dbPackages]  where pkgCode = 'ADVSECURITY')
	begin
		set @SEC =1
	end

	if @type = 'Client'
	begin
	
		if @Sec = 1
		begin
			INSERT INTO @T (DocumentID)
			SELECT 
				d.[DocID]
			FROM
				[config].[dbDocument] d
			WHERE D.ClID = @ID 
		end
		else
		begin
			INSERT INTO @T (DocumentID)
			SELECT 
				d.[DocID]
			FROM
				[config].[dbDocument] d
			WHERE D.ClID = @ID 
				and (not exists (select 1 from [relationship].[UserGroup_Document]RUGD JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] where DocumentID = d.docID and (PC.IsRemote = 0 or PC.IsRemote is Null))
					and not exists (select 1 from [relationship].[UserGroup_File]RUGF JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] where FileID = d.FileID and (PC.IsRemote = 0 or PC.IsRemote is Null)))
			union
			SELECT 
				RUGD.DocumentID
			FROM
				[relationship].[UserGroup_Document] RUGD 
			JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] 
			JOIN
				[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGD.[UserGroupID] = UGM.[ID]
			WHERE RUGD.ClID = @ID 
			and (PC.IsRemote = 0 or PC.IsRemote is Null) 
			group by RUGD.DocumentID
			having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0  
			option (MERGE UNION,OPTIMIZE for (@ID UNKNOWN))

			if exists ( select 1 from dbRegInfo where regBlockInheritence = 1)
			BEGIN
				INSERT INTO @T (DocumentID)
				SELECT 
					D.DocID
				FROM
					[config].[dbDocument] D
				JOIN
					[relationship].[UserGroup_File] RUGF ON RUGF.FileID = D.fileID
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] 
				JOIN
					[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGF.[UserGroupID] = UGM.[ID]
				LEFT JOIN @T ED ON ED.DocumentID = D.docID
				WHERE RUGF.FileID in (select fileid from config.dbfile where clid = @ID )
				and (PC.IsRemote = 0 or PC.IsRemote is Null) 
				and ED.DocumentID is null
				group by D.DocID
				having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0  
				option (MERGE UNION,OPTIMIZE for (@ID UNKNOWN))
			end


		end
	end

	else if @type = 'File'
	begin
	 
		if @Sec = 1
		begin
			INSERT INTO @T (DocumentID)
			SELECT 
				d.[DocID]
			FROM
				[config].[dbDocument] d
			WHERE D.FileID = @ID 
		end
		else
		begin
				INSERT INTO @T (DocumentID)
				SELECT 
					d.[DocID]
				FROM
					[config].[dbDocument] d
				WHERE D.FileID = @ID 
					and (not exists (select 1 from [relationship].[UserGroup_Document]RUGD JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] where DocumentID = d.docID and (PC.IsRemote = 0 or PC.IsRemote is Null))
					and not exists (select 1 from [relationship].[UserGroup_File]RUGF JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] where FileID = d.FileID and (PC.IsRemote = 0 or PC.IsRemote is Null)))
				union
				SELECT 
					RUGD.DocumentID
				FROM
					[relationship].[UserGroup_Document] RUGD 
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] 
				JOIN
					[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGD.[UserGroupID] = UGM.[ID]
				WHERE RUGD.FileID = @ID 
				and (PC.IsRemote = 0 or PC.IsRemote is Null) 
				group by RUGD.DocumentID
				having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0  
				option (MERGE UNION,OPTIMIZE for (@ID UNKNOWN))

			if exists ( select 1 from dbRegInfo where regBlockInheritence = 1)
			BEGIN
				INSERT INTO @T (DocumentID)
				SELECT 
					D.DocID
				FROM
					[config].[dbDocument] D
				JOIN
					[relationship].[UserGroup_File] RUGF ON RUGF.FileID = D.fileID
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] 
				JOIN
					[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGF.[UserGroupID] = UGM.[ID]
				LEFT JOIN @T ED ON ED.DocumentID = D.docID
				WHERE RUGF.FileID = @ID 
				and (PC.IsRemote = 0 or PC.IsRemote is Null) 
				and ED.DocumentID is null
				group by D.DocID
				having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0  
				option (MERGE UNION,OPTIMIZE for (@ID UNKNOWN))
			end
		end
	end

	else if @type = 'Document'
	begin
	
		if @Sec = 1
		begin
			INSERT INTO @T (DocumentID)
			SELECT 
				d.[DocID]
			FROM
				[config].[dbDocument] d
			WHERE D.DocID = @ID 
		end
		else
		begin

			INSERT INTO @T (DocumentID)
			SELECT 
				d.[DocID]
			FROM
				[config].[dbDocument] d
			WHERE D.DocID = @ID 
				and (not exists (select 1 from [relationship].[UserGroup_Document]RUGD JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] where DocumentID = d.docID and (PC.IsRemote = 0 or PC.IsRemote is Null))
					and not exists (select 1 from [relationship].[UserGroup_File]RUGF JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] where FileID = d.FileID and (PC.IsRemote = 0 or PC.IsRemote is Null)))
			union
			SELECT 
				RUGD.DocumentID
			FROM
				[relationship].[UserGroup_Document] RUGD 
			JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] 
			JOIN
				[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGD.[UserGroupID] = UGM.[ID]
			WHERE RUGD.DocumentID = @ID 
			and (PC.IsRemote = 0 or PC.IsRemote is Null) 
			group by RUGD.DocumentID
			having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0  
			option (MERGE UNION,OPTIMIZE for (@ID UNKNOWN))

			if exists ( select 1 from dbRegInfo where regBlockInheritence = 1)
			BEGIN
				INSERT INTO @T (DocumentID)
				SELECT 
					D.DocID
				FROM
					[config].[dbDocument] D 
				JOIN
					[relationship].[UserGroup_File] RUGF on RUGF.FileID = d.fileID
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] 
				JOIN
					[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGF.[UserGroupID] = UGM.[ID]
				WHERE D.docID =  @ID 
				and (PC.IsRemote = 0 or PC.IsRemote is Null) 
				group by D.DocID
				having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0  
				option (MERGE UNION,OPTIMIZE for (@ID UNKNOWN))
			end


		end
	end

	else  if @type = 'Contact'

	begin
		if @Sec = 1
		begin
			INSERT INTO @T (DocumentID)
			SELECT 
				d.[DocID]
			FROM
				[config].[dbDocument] d 
				join [dbo].[dbAssociates] cc
			on cc.associd = d.associd
			WHERE cc.contID = @ID 
		end
		else
		begin
			INSERT INTO @T (DocumentID)
			SELECT 
				d.[DocID]
			FROM
				[config].[dbDocument] d
				join [dbo].[dbAssociates] cc
			on cc.associd = d.associd
			WHERE cc.contID = @ID 
			and (not exists (select 1 from [relationship].[UserGroup_Document]RUGD JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] where DocumentID = d.docID and (PC.IsRemote = 0 or PC.IsRemote is Null))
			and not exists (select 1 from [relationship].[UserGroup_File]RUGF JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] where FileID = d.FileID and (PC.IsRemote = 0 or PC.IsRemote is Null)))

			union
			SELECT 
				RUGD.DocumentID
			FROM
				[relationship].[UserGroup_Document] RUGD 
			join	config.dbdocument D on rugd.documentID = d.docid
			join [dbo].[dbAssociates] cc	on cc.associd = d.associd
			JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] 
			JOIN
				[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGD.[UserGroupID] = UGM.[ID]
			WHERE cc.contID = @ID 
			and (PC.IsRemote = 0 or PC.IsRemote is Null) 
			group by RUGD.DocumentID
			having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0
			option (MERGE UNION,OPTIMIZE for (@ID UNKNOWN))

			if exists ( select 1 from dbRegInfo where regBlockInheritence = 1)
			BEGIN
				INSERT INTO @T (DocumentID)
				SELECT 
					D.DocID
				FROM
					[config].[dbDocument] D
				JOIN
					[relationship].[UserGroup_File] RUGF ON RUGF.FileID = D.fileID
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] 
				JOIN
					[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGF.[UserGroupID] = UGM.[ID]
				LEFT JOIN @T ED ON ED.DocumentID = D.docID
				WHERE RUGF.FileID in (select fileid from config.dbAssociates where contID = @ID ) 
				and (PC.IsRemote = 0 or PC.IsRemote is Null) 
				and ED.DocumentID is null
				group by D.DocID
				having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0  
				option (MERGE UNION,OPTIMIZE for (@ID UNKNOWN))
			end

		end
	end
	else
		if @Sec = 1
		begin
			INSERT INTO @T (DocumentID)
			SELECT 
				d.[DocID]
			FROM
				[config].[dbDocument] d 
		end
		else

		begin
			INSERT INTO @T (DocumentID)
			SELECT d.DocID
			FROM config.dbDocument d
			WHERE ISNULL((SELECT TOP 1 ISNULL(PC.IsRemote, 0) FROM [relationship].[UserGroup_Document] RUGD JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] WHERE DocumentID = d.docID ORDER BY PC.IsRemote), 1) = 1
				AND ISNULL((SELECT TOP 1 ISNULL(PC.IsRemote, 0) from [relationship].[UserGroup_File] RUGF JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] WHERE FileID = d.FileID ORDER BY PC.IsRemote), 1) = 1 
			union
			SELECT 
				RUGD.DocumentID
			FROM
				[relationship].[UserGroup_Document] RUGD 
			JOIN
				[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID] 
			JOIN
				[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGD.[UserGroupID] = UGM.[ID]
			WHERE (PC.IsRemote = 0 or PC.IsRemote is Null) 
			group by RUGD.DocumentID
			having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0  


			if exists ( select 1 from dbRegInfo where regBlockInheritence = 1)
			BEGIN
				INSERT INTO @T (DocumentID)
				SELECT 
					D.DocID
				FROM
					[config].[dbDocument] D
				JOIN
					[relationship].[UserGroup_File] RUGF ON RUGF.FileID = D.fileID
				JOIN
					[config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID] 
				JOIN
					[config].[GetUserAndGroupMembershipNT] (@USER) UGM ON RUGF.[UserGroupID] = UGM.[ID]
				LEFT JOIN @T ED ON ED.DocumentID = D.docID
				WHERE (PC.IsRemote = 0 or PC.IsRemote is Null) 
				and ED.DocumentID is null
				group by D.DocID
				having sum(Substring ( PC.AllowMask , 9 , 1 ) & 128) > 0 and sum(Substring ( PC.DenyMask , 9 , 1 ) & 128) = 0  

			end

		end

	return
end