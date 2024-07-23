CREATE PROCEDURE dbo.MergeUsers (@Source AS dbo.UsersMSType READONLY)
AS
SET NOCOUNT ON;

MERGE dbo.Users AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, usrinits, usralias, usrad, usrsql, usrfullname, usractive, modifieddate, usrAccessList)
	VALUES(source.mattersphereid, source.usrinits, source.usralias, source.usrad, source.usrsql, source.usrfullname, source.usractive, source.modifieddate, source.usrAccessList)
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.usrinits = source.usrinits
		, target.usralias = source.usralias
		, target.usrad = source.usrad
		, target.usrsql = source.usrsql
		, target.usrfullname = source.usrfullname
		, target.usractive = source.usractive
		, target.modifieddate = source.modifieddate
		, target.usrAccessList = source.usrAccessList
;
