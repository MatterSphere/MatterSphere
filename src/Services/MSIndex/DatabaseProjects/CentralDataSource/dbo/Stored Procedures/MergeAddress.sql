CREATE PROCEDURE dbo.MergeAddress (@Source AS dbo.AddressMSType READONLY)
AS
SET NOCOUNT ON;

MERGE dbo.Address AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, sc, modifieddate)
	VALUES(source.mattersphereid, source.sc, source.modifieddate)
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.mattersphereid = source.mattersphereid
		, target.sc = source.sc
		, target.modifieddate = source.modifieddate
;



