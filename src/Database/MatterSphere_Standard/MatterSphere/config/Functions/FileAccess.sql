CREATE FUNCTION [config].[FileAccess] ( )
RETURNS @T TABLE (ClID bigint,FileID bigINT)
AS
BEGIN
        DECLARE @USER NVARCHAR(200)
                        SET @USER = config.GetUserLogin()  
        DECLARE @Sec INT
        SELECT @SEC = [config].[IsAdministrator] (@User)
        IF @Sec = 0
        BEGIN
            INSERT @T
            SELECT  DISTINCT                             --Distinct added 2017-02-06 1625 - D.Abram - reduce the rows as many files have many policies
                            RUGF.CLID,RUGF.FILEID
            FROM
                            [relationship].[UserGroup_File] RUGF
            WHERE NOT EXISTS (SELECT
                            1
            FROM
                            [relationship].[UserGroup_File] F
            JOIN
                            [config].[GetUserAndGroupMembershipNT] (@User) UGM
                            ON UGM.ID = F.UserGroupID
            WHERE F.FileID = RUGF.FILEID)
            UNION
            SELECT DISTINCT
                            RUGF.CLID,RUGF.[FileID]
            FROM
                            [relationship].[UserGroup_File] RUGF
            JOIN
                            [config].[ObjectPolicy] PC ON PC.ID = RUGF.[PolicyID]
            WHERE (PC.IsRemote = 0 OR PC.IsRemote IS NULL)
            AND SUBSTRING ( PC.DenyMask , 6 , 1 ) & 32 = 32
            AND EXISTS (SELECT ID FROM [config].[GetUserAndGroupMembershipNT] (@User) UGM WHERE id = rugf.UserGroupID) 
			option (RECOMPILE)                               
	END
	RETURN
END