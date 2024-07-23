CREATE FUNCTION [config].[ClientFileAccess] (@clid bigint)
RETURNS @T TABLE (ClID bigint,FileID bigINT)
AS
BEGIN
        DECLARE @USER NVARCHAR(200)
                        SET @USER = config.GetUserLogin()  
		DECLARE @PERMID TABLE
		(ID uniqueidentifier)

		INSERT INTO @PERMID SELECT ID FROM [config].[GetUserAndGroupMembershipNT] (@USER) UGM				

		DECLARE @POLICYID TABLE
		(ID uniqueidentifier)

		INSERT into @POLICYID	select ID from [config].[ObjectPolicy] PC 
				WHERE (PC.IsRemote = 0 OR PC.IsRemote IS NULL)
				AND SUBSTRING ( PC.DenyMask , 6 , 1 ) & 32 = 32


        DECLARE @Sec INT
        SELECT @SEC = [config].[IsAdministrator] (@User)
        IF @Sec = 0
        BEGIN
				INSERT @T
				SELECT  DISTINCT                            
								RUGF.CLID,RUGF.FILEID
				FROM
								[relationship].[UserGroup_File] RUGF
				WHERE NOT EXISTS (SELECT top 1
								1
				FROM
								[relationship].[UserGroup_File] F
				JOIN
								@PERMID UGM
								ON UGM.ID = F.UserGroupID
				WHERE F.FileID = RUGF.FILEID)
				AND clid = @CLID
				UNION
				SELECT DISTINCT
								RUGF.CLID,RUGF.[FileID]
				FROM
								[relationship].[UserGroup_File] RUGF
				JOIN
								@POLICYID PC ON PC.ID = RUGF.[PolicyID]
				WHERE EXISTS (SELECT 1 FROM @PERMID WHERE id = rugf.UserGroupID) 
				AND clid = @CLID
				option (RECOMPILE)  
	END
	RETURN
END


GO


