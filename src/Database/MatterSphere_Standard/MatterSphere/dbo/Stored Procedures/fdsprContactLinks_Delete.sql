

CREATE PROCEDURE [dbo].[fdsprContactLinks_Delete]
(	@PARENTCONTID AS bigint,
	@SECCONTID AS bigint
)
AS
BEGIN
	-- Only run if both Contact IDs have values
	IF (@PARENTCONTID IS NOT NULL AND @SECCONTID IS NOT NULL)
		BEGIN
			-- First Link Deletion (i.e A is related to B)
			DELETE FROM DbContactLinks
			WHERE ContID = @PARENTCONTID AND ContLinkID = @SECCONTID

			-- Second Link Deletion (i.e B is related to A)
			DELETE FROM DbContactLinks
			WHERE ContLinkID = @PARENTCONTID AND ContID = @SECCONTID
	END
	-- Required for Datalist
	SELECT 0,0
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_Delete] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_Delete] TO [OMSAdminRole]
    AS [dbo];

