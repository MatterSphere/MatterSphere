-- Example Data for EsIndex 
INSERT INTO  search.ESIndex(EsIndexID, ESIndexName, ESIndexType)
SELECT EsIndexID
	, ESIndexName
	, ESIndexType
FROM (
VALUES (1, 'eliteindex1' ,'USER')
	, (2, 'eliteindex2', 'DATA')
) v (EsIndexID, ESIndexName, ESIndexType)
WHERE NOT EXISTS (SELECT 1 FROM search.ESIndex WHERE EsIndexID = v.EsIndexID)

IF EXISTS (SELECT TOP 1 0 FROM search.ESIndexTable WHERE ESIndexTableId = 7 AND ObjectType = N'ADDRESS')
BEGIN
	DELETE FROM [search].[ESIndexStructure] WHERE ESIndexTableId = 7
END

DECLARE @custom_schema NVARCHAR(20) = 'dbo.'
IF EXISTS (SELECT TOP 1 1 FROM sys.synonyms WHERE [schema_id] = schema_id('dbo') AND base_object_name LIKE '_config_.%')
	SET @custom_schema = 'config.'

MERGE INTO search.ESIndexTable AS Target 
USING (VALUES 
	(1, 2, N'DOCUMENT', CONCAT(@custom_schema, 'dbDocument'), 'docId', 1, 1, 1, '{clientNum}:{clientName} - {fileDesc}. {docDesc}.{documentExtension}'),
	(2, 1, N'USERS', N'dbo.dbUser', 'usrID', 1, 1, 1, ''),
	(3, 2, N'FILE', CONCAT(@custom_schema, 'dbFile'), 'fileId', 1, 1, 1, '{clientNum}:{clientName} - {fileDesc}'),
	(4, 2, N'CONTACT', CONCAT(@custom_schema, 'dbContact'), 'contId', 1, 1, 1, '{address}'),
	(5, 2, N'CLIENT', CONCAT(@custom_schema, 'dbClient'), 'clId', 1, 1, 1, '{clientType}'),
	(6, 2, N'ASSOCIATE', CONCAT(@custom_schema, 'dbAssociates'), 'assocId', 1, 1, 1, '{assocSalut} ({associateType})'),
	(9, 2, N'PRECEDENT', N'dbo.dbPrecedents', 'precId', 1, 1, 1, '{precCategory} ({precSubCategory}/{precMinorCategory}): {precDesc}'),
	(10, 2, N'CCLINK', N'dbo.dbClientContacts', 'Id', 1, 1, 1, ''),
	(11, 2, N'APPOINTMENT', N'dbo.dbAppointments', 'appID', 1, 0, 0, '{clientNum}:{clientName}/{fileDesc}: {appDesc} ({appointmentType}). Location: {appLocation}'),
	(12, 2, N'TASK', N'dbo.dbTasks', 'tskID', 1, 0, 0, '{fileDesc}: {tskDesc} ({taskType})'),
	(13, 2, N'EMAIL', CONCAT(@custom_schema, 'dbDocument'), 'docId', 1, 0, 0, '{clientNum}:{clientName}/{fileDesc}: {docDesc}'),
	(14, 2, N'NOTE', N'', '', 1, 0, 0, '{note}')
) 
AS Source (ESIndexTableId, ESIndexId, ObjectType, tablename, pkFieldName, FullCopyRequired, IndexingEnabled, IsDefault, summaryTemplate) 
ON Target.ESIndexTableId = Source.ESIndexTableId 
-- update matched rows 
WHEN MATCHED AND Target.summaryTemplate IS NULL THEN 
UPDATE SET ESIndexId = Source.ESIndexId,
	ObjectType = Source.ObjectType,
	tablename = Source.tablename,
	pkFieldName = source.pkFieldName,
	FullCopyRequired = source.FullCopyRequired,
	summaryTemplate = source.summaryTemplate,
--	IndexingEnabled = source.IndexingEnabled,
	IsDefault = source.IsDefault
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (ESIndexTableId, ESIndexId, ObjectType, tablename, pkFieldName, FullCopyRequired, IndexingEnabled, IsDefault, summaryTemplate)
VALUES (ESIndexTableId, ESIndexId, ObjectType, tablename, pkFieldName, FullCopyRequired, IndexingEnabled, IsDefault, summaryTemplate)
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN 
DELETE
;
GO

IF NOT EXISTS (SELECT 1 from search.ChangeVersionControl)
	INSERT INTO search.ChangeVersionControl (LastCopiedVersion, WorkingVersion, FullCopyRequired)
	VALUES (0,0,1)
ELSE
	UPDATE search.ChangeVersionControl SET FullCopyRequired = 1

--INSERT INTO  search.ESIndexStructure(ESIndexTableId, FieldName, ESFieldType, searchable, facetable, Analyzer, IsDefault, Suggestable)
MERGE INTO search.ESIndexStructure AS Target 
	USING(
	VALUES (1, 'client-id' ,'long', 0, 0, NULL, 0, NULL)
		, (1, 'clientNum', 'text', 0, 0, 'keyword', 0, NULL)
		, (1, 'clientName', 'text', 0, 0, 'english', 0, NULL)
		, (1, 'file-id', 'long', 0, 0, NULL, 0, NULL)
		, (1, 'fileDesc', 'text', 0, 0, 'english', 0, NULL)
		, (1, 'associate-id', 'long', 0, 0, NULL, 0, NULL)
		, (1, 'docDesc', 'text', 1, 0, 'english', 1, NULL)
		, (1, 'docContents', 'text', 1, 0, 'english', 0, NULL)
		, (1, 'docDeleted', 'boolean', 0, 0, NULL, 0, NULL)
		, (1, 'authorType', 'text', 1, 1, 'keyword', 0, 'Author')
		, (1, 'documentExtension', 'text', 1, 1, 'keyword', 0, 'DocExtension')
		, (1, 'documentType', 'text', 1, 1, 'keyword', 0, 'document type')
		, (2, 'usrinits', 'text', 1, 0, NULL, 0, NULL)
		, (2, 'usralias', 'text', 1, 0, NULL, 0, NULL)
		, (2, 'usrad', 'text', 1, 0, 'keyword', 0, NULL)
		, (2, 'usrsql', 'text', 1, 0, 'keyword', 0, NULL)
		, (2, 'usrfullname', 'text', 1, 0, 'keyword', 0, NULL)
		, (2, 'usractive', 'text', 1, 0, 'keyword', 0, NULL)
		, (3, 'client-id', 'long', 0, 0, NULL, 0, NULL)
		, (3, 'clientNum', 'text', 0, 0, 'keyword', 0, NULL)
		, (3, 'clientName', 'text', 0, 0, 'english', 0, NULL)
		, (3, 'fileDesc', 'text', 1, 0, 'english', 1, NULL)
		, (3, 'fileType', 'text', 1, 1, 'keyword', 0, 'file type')
		, (3, 'fileStatus', 'text', 1, 1, 'keyword', 0, 'file status')
		, (4, 'contactType', 'text', 1, 1, 'keyword', 0, 'contact type')
		, (4, 'contName', 'text', 1, 0, 'english', 1, NULL)
		, (4, 'address-id', 'long', 0, 0, NULL, 0, NULL)
		, (4, 'address', 'text', 1, 0, 'english', 1, NULL)
		, (5, 'name', 'text', 1, 0, 'english', 1, NULL)
		, (5, 'clientNotes', 'text', 1, 0, 'english', 0, NULL)
		, (5, 'clientType', 'text', 1, 1, 'keyword', 0, 'client type')
		, (5, 'address-id', 'long', 0, 0, NULL, 0, NULL)
		, (5, 'address', 'text', 1, 0, 'english', 1, NULL)
		, (5, 'clientNum', 'text', 1, 0, 'keyword', 0, NULL)
		, (6, 'file-id', 'long', 0, 0, NULL, 0, NULL)
		, (6, 'contact-id', 'long', 0, 0, NULL, 0, NULL)
		, (6, 'associateType', 'text', 1, 1, 'keyword', 0, 'associate type')
		, (6, 'assocHeading', 'text', 1, 0, 'english', 1, NULL)
		, (6, 'assocSalut', 'text', 1, 0, 'english', 1, NULL)
		, (6, 'assocAddressee', 'text', 1, 0, 'english', 1, NULL)
		, (9, 'precTitle', 'text', 1, 0, 'english', 1, NULL)
		, (9, 'precedentType', 'text', 1, 1, 'keyword', 0, 'precedent type')
		, (9, 'precLibrary', 'text', 1, 0, NULL, 1, NULL)
		, (9, 'precCategory', 'text', 1, 0, 'keyword', 0, NULL)
		, (9, 'precSubCategory', 'text', 1, 0, 'keyword', 0, NULL)
		, (9, 'precMinorCategory', 'text', 1, 0, 'keyword', 0, NULL)
		, (9, 'precDesc', 'text', 1, 0, 'english', 1, NULL)
		, (9, 'docContents', 'text', 1, 0, 'english', 0, NULL)
		, (9, 'precDeleted', 'boolean', 0, 0, NULL, 0, NULL)
		, (9, 'precedentExtension', 'text', 1, 1, 'keyword', 0, 'PrecExtension')
		, (10, 'client-id', 'long', 0, 0, NULL, 0, NULL)
		, (10, 'contact-id', 'long', 0, 0, NULL, 0, NULL)
		, (11, 'client-id', 'long', 0, 0, NULL, 0, NULL)
		, (11, 'clientNum', 'text', 0, 0, 'keyword', 0, NULL)
		, (11, 'clientName', 'text', 0, 0, 'english', 0, NULL)
		, (11, 'file-id', 'long', 0, 0, NULL, 0, NULL)
		, (11, 'fileDesc', 'text', 0, 0, 'english', 0, NULL)
		, (11, 'associateId', 'long', 0, 0, NULL, 0, NULL)
		, (11, 'documentId', 'long', 0, 0, NULL, 0, NULL)
		, (11, 'appointmentType', 'text', 1, 1, 'keyword', 0, 'appointmenttype')
		, (11, 'appDesc', 'text', 1, 0, 'english', 1, NULL)
		, (11, 'appLocation', 'text', 1, 0, 'english', 1, NULL)
		, (11, 'appDate', 'date', 0, 0, NULL, 0, NULL)
		, (11, 'appEndDate', 'date', 0, 0, NULL, 0, NULL)
		, (11, 'appTimeZone', 'text', 0, 0, 'english', 0, NULL)
		, (12, 'file-id', 'long', 0, 0, NULL, 0, NULL)
		, (12, 'fileDesc', 'text', 0, 0, 'english', 0, NULL)
		, (12, 'documentId', 'long', 0, 0, NULL, 0, NULL)
		, (12, 'taskType', 'text', 1, 1, 'keyword', 0, 'task type')
		, (12, 'tskDesc', 'text', 1, 0, 'english', 1, NULL)
		, (13, 'client-id' ,'long', 0, 0, NULL, 0, NULL)
		, (13, 'clientNum', 'text', 0, 0, 'keyword', 0, NULL)
		, (13, 'clientName', 'text', 0, 0, 'english', 0, NULL)
		, (13, 'file-id', 'long', 0, 0, NULL, 0, NULL)
		, (13, 'fileDesc', 'text', 0, 0, 'english', 0, NULL)
		, (13, 'associate-id', 'long', 0, 0, NULL, 0, NULL)
		, (13, 'docDesc', 'text', 1, 0, 'english', 1, NULL)
		, (13, 'docContents', 'text', 1, 0, 'english', 0, NULL)
		, (13, 'docDeleted', 'boolean', 0, 0, NULL, 0, NULL)
		, (13, 'authorType', 'text', 1, 1, 'keyword', 0, 'Author')
		, (13, 'documentExtension', 'text', 1, 1, 'keyword', 0, 'DocExtension')
		, (13, 'documentType', 'text', 1, 1, 'keyword', 0, 'document type')
		, (13, 'docFrom', 'text', 1, 0, 'english', 1, NULL)
		, (13, 'docTo', 'text', 1, 0, 'english', 1, NULL)
		, (13, 'docCC', 'text', 1, 0, 'english', 1, NULL)
		, (13, 'docSent', 'date', 0, 0, NULL, 0, NULL)
		, (13, 'docReceived', 'date', 0, 0, NULL, 0, NULL)
		, (13, 'docAttachments', 'long', 0, 0, NULL, 0, NULL)
		, (13, 'docConversationTopic', 'text', 1, 0, 'keyword', 0, NULL)
		, (14, 'noteSource' ,'text', 0, 0, NULL, 0, NULL)
		, (14, 'contactId', 'long', 0, 0, NULL, 0, NULL)
		, (14, 'clientId', 'long', 0, 0, NULL, 0, NULL)
		, (14, 'fileId', 'long', 0, 0, NULL, 0, NULL)
		, (14, 'associateId', 'text', 0, 0, NULL, 0, NULL)
		, (14, 'documentId', 'long', 0, 0, NULL, 0, NULL)
		, (14, 'appoinmentId', 'long', 0, 0, NULL, 0, NULL)
		, (14, 'taskId', 'long', 0, 0, NULL, 0, NULL)
		, (14, 'documentExtension', 'text', 1, 1, 'keyword', 0, 'DocExtension')
		, (14, 'note', 'text', 1, 0, 'english', 0, NULL)
		, (14, 'extNote', 'text', 1, 0, 'english', 0, NULL)
	) AS Source (ESIndexTableId, FieldName, ESFieldType, searchable, facetable, Analyzer, Suggestable, cdCode)
	ON Target.ESIndexTableId = Source.ESIndexTableId AND Target.FieldName = Source.FieldName
	-- update matched rows 
	--WHEN MATCHED THEN 
	--UPDATE SET ESFieldType = Source.ESFieldType
	--	, searchable = Source.searchable
	--	, facetable = Source.facetable
	--	, Analyzer = Source.Analyzer
	--	, IsDefault = 1
	--	, Suggestable = Source.Suggestable
	--	, cdCode = Source.cdCode
	-- insert new rows 
	WHEN NOT MATCHED BY TARGET THEN 
	INSERT (ESIndexTableId, FieldName, ESFieldType, searchable, facetable, Analyzer, IsDefault, Suggestable, cdCode)
	VALUES (ESIndexTableId, FieldName, ESFieldType, searchable, facetable, Analyzer, 1, Suggestable, cdCode)
	-- delete rows that are in the target but not the source 
	--WHEN NOT MATCHED BY SOURCE THEN 
	--DELETE
	;

--INSERT INTO  search.ESIndexCommonStructure(ESIndexId, FieldName, ESFieldType, searchable, facetable, Analyzer, cdCode)
MERGE INTO search.ESIndexCommonStructure AS Target 
	USING(
	VALUES 
		(1, 'id', 'text', 0, 0, 'whitespace', NULL)
		, (1, 'modifieddate', 'date', 0, 0, NULL, NULL)
		, (1, 'objecttype', 'text', 1, 1, 'keyword', N'MCTYPE')
		, (1, 'mattersphereid', 'text', 1, 0, 'standard', NULL)
		, (1, 'usrAccessList', 'text', 1, 0, 'whitespace', NULL)
		, (2, 'id', 'text', 0, 0, 'whitespace', NULL)
		, (2, 'modifieddate', 'date', 0, 0, NULL, NULL)
		, (2, 'objecttype', 'text', 1, 1, 'keyword', N'MCTYPE')
		, (2, 'title', 'text', 1, 0, 'english', NULL)
		, (2, 'summary', 'text', 1, 0, 'english', NULL)
		, (2, 'mattersphereid', 'text', 1, 0, 'standard', NULL)
		, (2, 'ugdp', 'text', 1, 0, 'whitespace', NULL)
	) AS Source (ESIndexId, FieldName, ESFieldType, searchable, facetable, Analyzer, cdCode)
	ON Target.ESIndexId = Source.ESIndexId AND Target.FieldName = Source.FieldName
	-- update matched rows 
	WHEN MATCHED THEN 
	UPDATE SET ESFieldType = Source.ESFieldType
		, searchable = Source.searchable
		, facetable = Source.facetable
		, Analyzer = Source.Analyzer
		, cdCode = Source.cdCode
	-- insert new rows 
	WHEN NOT MATCHED BY TARGET THEN 
	INSERT (ESIndexId, FieldName, ESFieldType, searchable, facetable, Analyzer, cdCode)
	VALUES (ESIndexId, FieldName, ESFieldType, searchable, facetable, Analyzer, cdCode)
	-- delete rows that are in the target but not the source 
	WHEN NOT MATCHED BY SOURCE THEN 
	DELETE
	;

--IF EXISTS(SELECT 1 FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'dbo.dbAppointments')) ALTER TABLE dbo.dbAppointments DISABLE CHANGE_TRACKING
--IF EXISTS(SELECT 1 FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'dbo.dbTasks')) ALTER TABLE dbo.dbTasks DISABLE CHANGE_TRACKING


INSERT INTO dbo.dbSpecificData ( brID, spLookup, spData)
SELECT b.brID
	, v.spLookup
	, v.spData
FROM dbo.dbBranch b
	CROSS JOIN (
		VALUES('ES_SERV', 'http://localhost:9200')
		, ('ES_DIND', 'eliteindex2')
		, ('ES_UIND', 'eliteindex1')
		, ('ES_FACSIZE', '10')
		, ('ES_APIKEY', '')
		) v (spLookup, spData)
WHERE NOT EXISTS (SELECT 1 FROM dbo.dbSpecificData WHERE spLookup = v.spLookup)
