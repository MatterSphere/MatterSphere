DELETE FROM [search].[IndexedEntity] WHERE EntityName = N'Address'

INSERT INTO search.IndexedEntity(EntityName, FullCopyRequired)
SELECT EntityName
	, 1 AS FullCopyRequired
FROM
	(VALUES (N'Document'),
	  (N'Users'),
	  (N'File'),
	  (N'Contact'),
	  (N'Client'),
	  (N'Associate'),
	  (N'Precedent'),
	  (N'Appointment'),
	  (N'Task')
	) AS Source (EntityName) 
WHERE NOT EXISTS(SELECT 1 FROM search.IndexedEntity WHERE EntityName = Source.EntityName)

IF NOT EXISTS (SELECT 1 from search.ChangeVersionControl)
	INSERT INTO search.ChangeVersionControl (LastCopiedVersion, WorkingVersion, FullCopyRequired)
	VALUES (0,0,1)
ELSE
	UPDATE search.ChangeVersionControl SET FullCopyRequired = 1
