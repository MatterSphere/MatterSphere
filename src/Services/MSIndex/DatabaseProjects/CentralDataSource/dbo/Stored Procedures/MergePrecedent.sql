CREATE PROCEDURE dbo.MergePrecedent (@Source AS dbo.PrecedentMSType READONLY)
AS
SET NOCOUNT ON;

MERGE dbo.Precedent AS target
USING @Source AS source ON (target.mattersphereid = source.mattersphereid)
WHEN NOT MATCHED BY TARGET THEN
	INSERT (mattersphereid, precDesc, precTitle, precLibrary, precSubCategory, modifieddate, precedentType, precCategory, precDeleted, precedentExtension)
	VALUES(source.mattersphereid, source.precDesc, source.precTitle, source.precLibrary, source.precSubCategory, source.modifieddate, source.precedentType, source.precCategory, source.precDeleted, source.precedentExtension)
WHEN MATCHED AND source.op = 'D'
    THEN DELETE  
WHEN MATCHED
	THEN UPDATE SET target.precDesc = source.precDesc
		, target.precTitle = source.precTitle
		, target.precLibrary = source.precLibrary
		, target.precSubCategory = source.precSubCategory
		, target.modifieddate = source.modifieddate
		, target.precedentType = source.precedentType
		, target.precCategory = source.precCategory
		, target.precDeleted = source.precDeleted
		, target.precedentExtension = source.precedentExtension
;
