CREATE PROCEDURE dbo.Search (
	@UserId INT
	, @SearchKey NVARCHAR(4000)
	, @SearchIn VARCHAR(1000)
	, @EntityFilter dbo.EntityType READONLY
	, @FacetFilter dbo.FacetType READONLY
	, @MAX_RECORDS INT = 50
	, @PageNo INT = 1
	, @IsSecurityAdmin BIT = 0

)
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @UGDP TABLE(UserGroupID UNIQUEIDENTIFIER PRIMARY KEY)
DECLARE @Entity TABLE(EntityName VARCHAR(25) PRIMARY KEY)
DECLARE @ResId TABLE(EntityName VARCHAR(25), mattersphereid BIGINT, modifieddate DATETIME, PRIMARY KEY (modifieddate, EntityName, mattersphereid))
CREATE TABLE #FacetOut (mattersphereid BIGINT)
CREATE TABLE #FacetRes (FacetField VARCHAR(25), cdCode NVARCHAR(15), FacetValue NVARCHAR(200), Cnt BIGINT)

DECLARE @SearchIDName VARCHAR(25) = NULL, @SearchID BIGINT = NULL
SELECT @SearchIDName = EntityField, @SearchID = EntityValue FROM @EntityFilter WHERE EntityPrimary = 1

DECLARE @precedentType NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'precedentType')
	, @precedentExtension NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'precedentExtension')
	, @contactType NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'contactType')
	, @clientType NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'clientType')
	, @fileType NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'fileType')
	, @fileStatus NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'fileStatus')
	, @associateType NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'associateType')
	, @usrFullName NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'usrFullName')
	, @documentExtension NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'documentExtension')
	, @documentStartDate DATETIME = (SELECT CONVERT(DATETIME, FacetValue, 126) FROM @FacetFilter WHERE FacetField = 'documentStartDate')
	, @documentEndDate DATETIME = (SELECT CONVERT(DATETIME, FacetValue, 126) FROM @FacetFilter WHERE FacetField = 'documentEndDate')
	, @documentType NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'documentType')
	, @appointmentType NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'appointmentType')
	, @taskType NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'taskType')
	, @objectType NVARCHAR(200) = (SELECT FacetValue FROM @FacetFilter WHERE FacetField = 'objectType')

DECLARE @Total BIGINT = 0
	, @OFFSET INT = 0
	, @TOP INT
	, @SearchAll BIT = 0
	
IF @MAX_RECORDS > 0
	SET @TOP = @MAX_RECORDS
ELSE
	SET @TOP = 50

IF @PageNo IS NULL
	SET @OFFSET = 0
ELSE
	SET @OFFSET = @TOP * @PageNo

IF @SearchKey = '"*"' SET @SearchAll = 1

INSERT INTO @UGDP(UserGroupID)
SELECT ugdp.Value
FROM dbo.Users u
	CROSS APPLY dbo.SplitString(u.usrAccessList, ' ') ugdp
WHERE u.mattersphereid = @UserId

IF @objectType IS NULL
	INSERT INTO @Entity(EntityName)
	SELECT e.Value
	FROM dbo.SplitString(@SearchIn, ',') e
ELSE
	INSERT INTO @Entity(EntityName)
	VALUES(@objectType)

IF EXISTS(SELECT * FROM @Entity WHERE EntityName = 'Precedent')
BEGIN
	INSERT INTO @ResId (EntityName, mattersphereid, modifieddate)
	OUTPUT INSERTED.mattersphereid INTO #FacetOut
	SELECT 'Precedent'
		, s.mattersphereid
		, s.modifieddate
	FROM dbo.Precedent s
	WHERE ((1 = CASE
				WHEN @SearchAll = 1 THEN 1
				WHEN @SearchAll = 0 AND (CONTAINS(precSearch, @SearchKey)) THEN 1
				ELSE 0 END)
			AND @SearchID IS NULL)
		AND (@precedentType IS NULL OR s.precedentType = @precedentType)
		AND (@precedentExtension IS NULL OR s.precedentExtension = @precedentExtension)
		AND @contactType IS NULL
		AND @clientType IS NULL
		AND @fileType IS NULL
		AND @fileStatus IS NULL
		AND @associateType IS NULL
		AND @usrFullName IS NULL
		AND @documentExtension IS NULL
		AND @documentType IS NULL
		AND @appointmentType IS NULL
		AND @taskType IS NULL

	SET @Total = @Total + @@ROWCOUNT

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'precedentType'
		, 'precedent type'
		, prec.precedentType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Precedent prec ON prec.mattersphereid = f.mattersphereid
	GROUP BY prec.precedentType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'precedentExtension'
		, 'PrecExtension'
		, prec.precedentExtension
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Precedent prec ON prec.mattersphereid = f.mattersphereid
	GROUP BY prec.precedentExtension

	TRUNCATE TABLE #FacetOut
END

IF EXISTS(SELECT * FROM @Entity WHERE EntityName = 'Contact')
BEGIN
	INSERT INTO @ResId (EntityName, mattersphereid, modifieddate)
	OUTPUT INSERTED.mattersphereid INTO #FacetOut
	SELECT 'Contact'
		, s.mattersphereid
		, s.modifieddate
	FROM dbo.Contact s
		INNER JOIN dbo.Address a ON a.mattersphereid = s.[address-id]
		OUTER APPLY(SELECT SUM(CASE WHEN ugdp.UserGroupID = ug.UserGroupID THEN 0 ELSE 1 END) AS e, MAX(CAST(ug.gDeny AS TINYINT)) AS gDeny FROM dbo.UserGroup_Contact ug LEFT OUTER JOIN  @UGDP ugdp ON ugdp.UserGroupID = ug.UserGroupID WHERE ug.mattersphereid = s.mattersphereid AND @IsSecurityAdmin = 0) d
	WHERE ((1 = CASE
				WHEN @SearchAll = 1 THEN 1
				WHEN @SearchAll = 0 AND (CONTAINS(contSearch, @SearchKey) OR CONTAINS(a.sc, @SearchKey)) THEN 1
				ELSE 0 END)
			AND (@SearchID IS NULL OR (@SearchIDName = 'contact-id' AND s.mattersphereid = @SearchID) OR
				(@SearchIDName = 'client-id' AND s.mattersphereid IN (SELECT EntityValue FROM @EntityFilter WHERE EntityField = 'contact-id' AND EntityPrimary = 0))))
		AND (ISNULL(d.e, 0) = 0 AND ISNULL(d.gDeny, 0) = 0)
		AND (@contactType IS NULL OR s.contactType = @contactType)
		AND @precedentType IS NULL
		AND @precedentExtension IS NULL
		AND @clientType IS NULL
		AND @fileType IS NULL
		AND @fileStatus IS NULL
		AND @associateType IS NULL
		AND @usrFullName IS NULL
		AND @documentExtension IS NULL
		AND @documentType IS NULL
		AND @appointmentType IS NULL
		AND @taskType IS NULL

	SET @Total = @Total + @@ROWCOUNT

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'contactType'
		, 'contact type'
		, c.contactType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Contact c ON c.mattersphereid = f.mattersphereid
	GROUP BY c.contactType

	TRUNCATE TABLE #FacetOut
END

IF EXISTS(SELECT * FROM @Entity WHERE EntityName = 'Client')
BEGIN
	INSERT INTO @ResId (EntityName, mattersphereid, modifieddate)
	OUTPUT INSERTED.mattersphereid INTO #FacetOut
	SELECT 'Client'
		, s.mattersphereid
		, s.modifieddate
	FROM dbo.Client s
		LEFT OUTER JOIN dbo.Address a ON a.mattersphereid = s.[address-id]
		OUTER APPLY(SELECT SUM(CASE WHEN ugdp.UserGroupID = ug.UserGroupID THEN 0 ELSE 1 END) AS e, MAX(CAST(ug.gDeny AS TINYINT)) AS gDeny FROM dbo.UserGroup_Client ug LEFT OUTER JOIN  @UGDP ugdp ON ugdp.UserGroupID = ug.UserGroupID WHERE ug.mattersphereid = s.mattersphereid AND @IsSecurityAdmin = 0) d
	WHERE ((1 = CASE
				WHEN @SearchAll = 1 THEN 1
				WHEN @SearchAll = 0 AND (CONTAINS(clSearch, @SearchKey) OR CONTAINS(a.sc, @SearchKey)) THEN 1
				ELSE 0 END)
			AND (@SearchID IS NULL OR (@SearchIDName = 'client-id' AND s.mattersphereid = @SearchID)))
		AND (ISNULL(d.e, 0) = 0 AND ISNULL(d.gDeny, 0) = 0)
		AND (@clientType IS NULL OR s.clientType = @clientType)
		AND @precedentType IS NULL
		AND @precedentExtension IS NULL
		AND @contactType IS NULL
		AND @fileType IS NULL
		AND @fileStatus IS NULL
		AND @associateType IS NULL
		AND @usrFullName IS NULL
		AND @documentExtension IS NULL
		AND @documentType IS NULL
		AND @appointmentType IS NULL
		AND @taskType IS NULL

	SET @Total = @Total + @@ROWCOUNT

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'clientType'
		, 'client type'
		, c.clientType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Client c ON c.mattersphereid = f.mattersphereid
	GROUP BY c.clientType

	TRUNCATE TABLE #FacetOut
END

IF EXISTS(SELECT * FROM @Entity WHERE EntityName = 'File')
BEGIN
	INSERT INTO @ResId (EntityName, mattersphereid, modifieddate)
	OUTPUT INSERTED.mattersphereid INTO #FacetOut
	SELECT 'File'
		, s.mattersphereid
		, s.modifieddate
	FROM dbo.[File] s
		INNER JOIN dbo.Client c ON c.mattersphereid = s.[client-id]
		OUTER APPLY(SELECT SUM(CASE WHEN ugdp.UserGroupID = ug.UserGroupID THEN 0 ELSE 1 END) AS e, MAX(CAST(ug.gDeny AS TINYINT)) AS gDeny FROM dbo.UserGroup_File ug LEFT OUTER JOIN  @UGDP ugdp ON ugdp.UserGroupID = ug.UserGroupID WHERE ug.mattersphereid = s.mattersphereid  AND @IsSecurityAdmin = 0) d
	WHERE ((1 = CASE
				WHEN @SearchAll = 1 THEN 1
				WHEN @SearchAll = 0 AND (CONTAINS(fileSearch, @SearchKey) OR CONTAINS(clSearch, @SearchKey)) THEN 1
				ELSE 0 END)
			AND (@SearchID IS NULL OR (@SearchIDName = 'client-id' AND s.[client-id] = @SearchID) OR (@SearchIDName = 'file-id' AND s.mattersphereid = @SearchID)))
		AND (ISNULL(d.e, 0) = 0 AND ISNULL(d.gDeny, 0) = 0)
		AND (@fileType IS NULL OR s.fileType = @fileType)
		AND (@fileStatus IS NULL OR s.fileStatus = @fileStatus)
		AND (@clientType IS NULL OR c.clientType = @clientType)
		AND @precedentType IS NULL
		AND @precedentExtension IS NULL
		AND @contactType IS NULL
		AND @associateType IS NULL
		AND @usrFullName IS NULL
		AND @documentExtension IS NULL
		AND @documentType IS NULL
		AND @appointmentType IS NULL
		AND @taskType IS NULL

	SET @Total = @Total + @@ROWCOUNT

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileType'
		, 'file type'
		, fl.fileType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = f.mattersphereid
	GROUP BY fl.fileType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileStatus'
		, 'file status'
		, fl.fileStatus
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = f.mattersphereid
	GROUP BY fl.fileStatus

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'clientType'
		, 'client type'
		, c.clientType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = f.mattersphereid
		INNER JOIN dbo.Client c ON c.mattersphereid = fl.[client-id]
	GROUP BY c.clientType

	TRUNCATE TABLE #FacetOut
END

IF EXISTS(SELECT * FROM @Entity WHERE EntityName = 'Associate')
BEGIN
	INSERT INTO @ResId (EntityName, mattersphereid, modifieddate)
	OUTPUT INSERTED.mattersphereid INTO #FacetOut
	SELECT 'Associate'
		, s.mattersphereid
		, s.modifieddate
	FROM dbo.Associate s
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = s.[file-id]
		INNER JOIN dbo.Contact c ON c.mattersphereid = s.[contact-id]
		OUTER APPLY(SELECT SUM(CASE WHEN ugdp.UserGroupID = ug.UserGroupID THEN 0 ELSE 1 END) AS e, MAX(CAST(ug.gDeny AS TINYINT)) AS gDeny FROM dbo.UserGroup_Associate ug LEFT OUTER JOIN  @UGDP ugdp ON ugdp.UserGroupID = ug.UserGroupID WHERE ug.mattersphereid = s.mattersphereid AND @IsSecurityAdmin = 0) d
	WHERE ((1 = CASE
				WHEN @SearchAll = 1 THEN 1
				WHEN @SearchAll = 0 AND (CONTAINS(assocSearch, @SearchKey) OR CONTAINS(fileSearch, @SearchKey) OR CONTAINS(contSearch, @SearchKey)) THEN 1
				ELSE 0 END)
			AND (@SearchID IS NULL OR (@SearchIDName = 'contact-id' AND s.[contact-id] = @SearchID) OR (@SearchIDName = 'file-id' AND s.[file-id] = @SearchID) OR (@SearchIDName = 'associate-id' AND s.mattersphereid = @SearchID) OR (@SearchIDName = 'client-id' AND fl.[client-id] = @SearchID)))
		AND (ISNULL(d.e, 0) = 0 AND ISNULL(d.gDeny, 0) = 0)
		AND (@associateType IS NULL OR s.associateType = @associateType)
		AND (@fileType IS NULL OR fl.fileType = @fileType)
		AND (@fileStatus IS NULL OR fl.fileStatus = @fileStatus)
		AND (@contactType IS NULL OR c.contactType = @contactType)
		AND @precedentType IS NULL
		AND @precedentExtension IS NULL
		AND @clientType IS NULL
		AND @usrFullName IS NULL
		AND @documentExtension IS NULL
		AND @documentType IS NULL
		AND @appointmentType IS NULL
		AND @taskType IS NULL

	SET @Total = @Total + @@ROWCOUNT

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'associateType'
		, 'associate type'
		, a.associateType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Associate a ON a.mattersphereid = f.mattersphereid
	GROUP BY a.associateType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileType'
		, 'file type'
		, fl.fileType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Associate a ON a.mattersphereid = f.mattersphereid
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = a.[file-id]
	GROUP BY fl.fileType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileStatus'
		, 'file status'
		, fl.fileStatus
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Associate a ON a.mattersphereid = f.mattersphereid
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = a.[file-id]
	GROUP BY fl.fileStatus

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'contactType'
		, 'contact type'
		, c.contactType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Associate a ON a.mattersphereid = f.mattersphereid
		INNER JOIN dbo.Contact c ON c.mattersphereid = a.[contact-id]
	GROUP BY c.contactType

	TRUNCATE TABLE #FacetOut
END

IF EXISTS(SELECT * FROM @Entity WHERE EntityName = 'Document')
BEGIN
	INSERT INTO @ResId (EntityName, mattersphereid, modifieddate)
	OUTPUT INSERTED.mattersphereid INTO #FacetOut
	SELECT 'Document'
		, s.mattersphereid
		, s.modifieddate
	FROM dbo.Document s
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = s.[file-id]
		INNER JOIN dbo.Client c ON c.mattersphereid = s.[client-id]
		INNER JOIN dbo.Associate a ON a.mattersphereid = s.[associate-id]
		OUTER APPLY(SELECT SUM(CASE WHEN ugdp.UserGroupID = ug.UserGroupID THEN 0 ELSE 1 END) AS e, MAX(CAST(ug.gDeny AS TINYINT)) AS gDeny FROM dbo.UserGroup_Document ug LEFT OUTER JOIN  @UGDP ugdp ON ugdp.UserGroupID = ug.UserGroupID WHERE ug.mattersphereid = s.mattersphereid AND @IsSecurityAdmin = 0) d
	WHERE ((1 = CASE
				WHEN @SearchAll = 1 THEN 1
				WHEN @SearchAll = 0 AND (CONTAINS(docSearch, @SearchKey) OR CONTAINS(assocSearch, @SearchKey) OR CONTAINS(fileSearch, @SearchKey) OR CONTAINS(clSearch, @SearchKey)) THEN 1
				ELSE 0 END)
			AND (@SearchID IS NULL OR (@SearchIDName = 'client-id' AND s.[client-id] = @SearchID) OR (@SearchIDName = 'file-id' AND s.[file-id] = @SearchID)))
		AND (ISNULL(d.e, 0) = 0 AND ISNULL(d.gDeny, 0) = 0)
		AND (@usrFullName IS NULL OR s.usrFullName = @usrFullName)
		AND (@documentExtension IS NULL OR s.documentExtension = @documentExtension)
		AND (@documentStartDate IS NULL OR s.modifieddate >= @documentStartDate)
		AND (@documentEndDate IS NULL OR s.modifieddate <= @documentEndDate)
		AND (@documentType IS NULL OR s.documentType = @documentType)
		AND (@associateType IS NULL OR a.associateType = @associateType)
		AND (@fileType IS NULL OR fl.fileType = @fileType)
		AND (@fileStatus IS NULL OR fl.fileStatus = @fileStatus)
		AND (@clientType IS NULL OR c.clientType = @clientType)
		AND @precedentType IS NULL
		AND @precedentExtension IS NULL
		AND @appointmentType IS NULL
		AND @taskType IS NULL
		AND @contactType IS NULL

	SET @Total = @Total + @@ROWCOUNT

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'usrFullName'
		, 'Author'
		, d.usrFullName
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Document d ON d.mattersphereid = f.mattersphereid
	GROUP BY d.usrFullName

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'documentExtension'
		, 'DocExtension'
		, d.documentExtension
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Document d ON d.mattersphereid = f.mattersphereid
	GROUP BY d.documentExtension

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'documentType'
		, 'document type'
		, d.documentType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Document d ON d.mattersphereid = f.mattersphereid
	GROUP BY d.documentType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileType'
		, 'file type'
		, fl.fileType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Document d ON d.mattersphereid = f.mattersphereid
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = d.[file-id]
	GROUP BY fl.fileType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileStatus'
		, 'file status'
		, fl.fileStatus
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Document d ON d.mattersphereid = f.mattersphereid
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = d.[file-id]
	GROUP BY fl.fileStatus

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'clientType'
		, 'client type'
		, c.clientType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Document d ON d.mattersphereid = f.mattersphereid
		INNER JOIN dbo.Client c ON c.mattersphereid = d.[client-id]
	GROUP BY c.clientType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'associateType'
		, 'associate type'
		, a.associateType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Document d ON d.mattersphereid = f.mattersphereid
		INNER JOIN dbo.Associate a ON a.mattersphereid = d.[associate-id]
	GROUP BY a.associateType

	TRUNCATE TABLE #FacetOut
END

IF EXISTS(SELECT * FROM @Entity WHERE EntityName = 'Appointment')
BEGIN
	INSERT INTO @ResId (EntityName, mattersphereid, modifieddate)
	OUTPUT INSERTED.mattersphereid INTO #FacetOut
	SELECT 'Appointment'
		, s.mattersphereid
		, s.modifieddate
	FROM dbo.Appointment s
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = s.[file-id]
		INNER JOIN dbo.Client c ON c.mattersphereid = s.[client-id]
		OUTER APPLY(SELECT COUNT(*) AS e, MAX(CAST(ug.gDeny AS TINYINT)) AS gDeny FROM dbo.UserGroup_File ug LEFT OUTER JOIN  @UGDP ugdp ON ugdp.UserGroupID = ug.UserGroupID WHERE ug.mattersphereid = s.[file-id] AND @IsSecurityAdmin = 0) d
	WHERE ((1 = CASE
				WHEN @SearchAll = 1 THEN 1
				WHEN @SearchAll = 0 AND (CONTAINS(appSearch, @SearchKey) OR CONTAINS(fileSearch, @SearchKey) OR CONTAINS(clSearch, @SearchKey)) THEN 1
				ELSE 0 END)
			AND (@SearchID IS NULL OR (@SearchIDName = 'client-id' AND s.[client-id] = @SearchID) OR (@SearchIDName = 'file-id' AND s.[file-id] = @SearchID)))
		AND (ISNULL(d.e, 0) = 0 AND ISNULL(d.gDeny, 0) = 0)
		AND (@appointmentType IS NULL OR s.appointmentType = @appointmentType)
		AND (@fileType IS NULL OR fl.fileType = @fileType)
		AND (@fileStatus IS NULL OR fl.fileStatus = @fileStatus)
		AND (@clientType IS NULL OR c.clientType = @clientType)
		AND @precedentType IS NULL
		AND @precedentExtension IS NULL
		AND @taskType IS NULL
		AND @contactType IS NULL
		AND @associateType IS NULL
		AND @usrFullName IS NULL
		AND @documentExtension IS NULL
		AND @documentType IS NULL

	SET @Total = @Total + @@ROWCOUNT

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'appointmentType'
		, 'appointmenttype'
		, a.appointmentType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Appointment a ON a.mattersphereid = f.mattersphereid
	GROUP BY a.appointmentType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileType'
		, 'file type'
		, fl.fileType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Appointment a ON a.mattersphereid = f.mattersphereid
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = a.[file-id]
	GROUP BY fl.fileType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileStatus'
		, 'file status'
		, fl.fileStatus
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Appointment a ON a.mattersphereid = f.mattersphereid
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = a.[file-id]
	GROUP BY fl.fileStatus

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'clientType'
		, 'client type'
		, c.clientType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Appointment a ON a.mattersphereid = f.mattersphereid
		INNER JOIN dbo.Client c ON c.mattersphereid = a.[client-id]
	GROUP BY c.clientType

	TRUNCATE TABLE #FacetOut
END

IF EXISTS(SELECT * FROM @Entity WHERE EntityName = 'Task')
BEGIN
	INSERT INTO @ResId (EntityName, mattersphereid, modifieddate)
	OUTPUT INSERTED.mattersphereid INTO #FacetOut
	SELECT 'Task'
		, s.mattersphereid
		, s.modifieddate
	FROM dbo.Task s
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = s.[file-id]
		OUTER APPLY(SELECT COUNT(*) AS e, MAX(CAST(ug.gDeny AS TINYINT)) AS gDeny FROM dbo.UserGroup_File ug LEFT OUTER JOIN  @UGDP ugdp ON ugdp.UserGroupID = ug.UserGroupID WHERE ug.mattersphereid = s.[file-id] AND @IsSecurityAdmin = 0) d
	WHERE ((1 = CASE
				WHEN @SearchAll = 1 THEN 1
				WHEN @SearchAll = 0 AND (CONTAINS(tskSearch, @SearchKey) OR CONTAINS(fileSearch, @SearchKey)) THEN 1
				ELSE 0 END)
			AND (@SearchID IS NULL OR (@SearchIDName = 'file-id' AND s.[file-id] = @SearchID)))
		AND (ISNULL(d.e, 0) = 0 AND ISNULL(d.gDeny, 0) = 0)
		AND (@taskType IS NULL OR s.taskType = @taskType)
		AND (@fileType IS NULL OR fl.fileType = @fileType)
		AND (@fileStatus IS NULL OR fl.fileStatus = @fileStatus)
		AND @precedentType IS NULL
		AND @precedentExtension IS NULL
		AND @appointmentType IS NULL
		AND @associateType IS NULL
		AND @clientType IS NULL
		AND @contactType IS NULL
		AND @usrFullName IS NULL
		AND @documentExtension IS NULL
		AND @documentType IS NULL

	SET @Total = @Total + @@ROWCOUNT

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'taskType'
		, 'task type'
		, t.taskType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Task t ON t.mattersphereid = f.mattersphereid
	GROUP BY t.taskType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileType'
		, 'file type'
		, fl.fileType
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Task t ON t.mattersphereid = f.mattersphereid
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = t.[file-id]
	GROUP BY fl.fileType

	INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
	SELECT 'fileStatus'
		, 'file status'
		, fl.fileStatus
		, COUNT(*)
	FROM #FacetOut f
		INNER JOIN dbo.Task t ON t.mattersphereid = f.mattersphereid
		INNER JOIN dbo.[File] fl ON fl.mattersphereid = t.[file-id]
	GROUP BY fl.fileStatus

	TRUNCATE TABLE #FacetOut
END;

WITH RN AS
(
	SELECT EntityName 
		, mattersphereid
		, modifieddate
		, ROW_NUMBER() OVER(ORDER BY modifieddate, EntityName, mattersphereid) AS id
	FROM @ResId
)
SELECT TOP(@TOP) 
	rn.EntityName
	, rn.mattersphereid AS Id
	, rn.modifieddate
	, CASE rn.EntityName 
		WHEN 'Contact' THEN (SELECT a.sc FROM dbo.Address a WHERE a.mattersphereid = cont.[address-id])
	END AS Address
	, CASE rn.EntityName WHEN 'Precedent' THEN prec.precTitle END AS PrecedentTitle
	, CASE rn.EntityName WHEN 'Precedent' THEN prec.precCategory END AS PrecedentCategory
	, CASE rn.EntityName WHEN 'Precedent' THEN prec.precSubCategory END AS PrecedentSubcategory
	, CASE rn.EntityName WHEN 'Precedent' THEN prec.precDesc END AS PrecedentDescription
	, CASE rn.EntityName WHEN 'Precedent' THEN prec.precedentExtension END AS PrecedentExtension
	, CASE rn.EntityName WHEN 'Contact' THEN cont.contName END AS ContactName
	, CASE rn.EntityName 
		WHEN 'Client' THEN cl.clNo 
		WHEN 'File' THEN (SELECT c.clNo FROM dbo.Client c WHERE c.mattersphereid = f.[client-id])
		WHEN 'Document' THEN (SELECT c.clNo FROM dbo.Client c WHERE c.mattersphereid = doc.[client-id])
		WHEN 'Appointment' THEN (SELECT c.clNo FROM dbo.Client c WHERE c.mattersphereid = app.[client-id])
	END AS ClientNumber
	, CASE rn.EntityName 
		WHEN 'Client' THEN cl.clName 
		WHEN 'File' THEN (SELECT c.clName FROM dbo.Client c WHERE c.mattersphereid = f.[client-id])
		WHEN 'Document' THEN (SELECT c.clName FROM dbo.Client c WHERE c.mattersphereid = doc.[client-id])
		WHEN 'Appointment' THEN (SELECT c.clName FROM dbo.Client c WHERE c.mattersphereid = app.[client-id])
	END AS ClientName
	, CASE rn.EntityName WHEN 'Client' THEN cl.clientType END AS ClientType
	, CASE rn.EntityName 
		WHEN 'File' THEN f.fileDesc 
		WHEN 'Document' THEN (SELECT fd.fileDesc FROM dbo.[File] fd WHERE fd.mattersphereid = doc.[file-id])
		WHEN 'Appointment' THEN (SELECT fd.fileDesc FROM dbo.[File] fd WHERE fd.mattersphereid = app.[file-id])
		WHEN 'Task' THEN (SELECT fd.fileDesc FROM dbo.[File] fd WHERE fd.mattersphereid = tsk.[file-id])
	END AS FileDescription
	, CASE rn.EntityName 
		WHEN 'File' THEN f.[client-id] 
		WHEN 'Document' THEN doc.[client-id] 
		WHEN 'Appointment' THEN app.[client-id] 
	END AS ClientId
	, CASE rn.EntityName WHEN 'Associate' THEN assoc.assocSalut END AS Salutation
	, CASE rn.EntityName WHEN 'Associate' THEN assoc.associateType END AS AssociateType
 	, CASE rn.EntityName WHEN 'Associate' THEN assoc.[contact-id] END AS ContactId
	, CASE rn.EntityName 
		WHEN 'Associate' THEN assoc.[file-id] 
		WHEN 'Document' THEN doc.[file-id] 
		WHEN 'Appointment' THEN app.[file-id] 
		WHEN 'Task' THEN tsk.[file-id] 
	END AS FileId
	, CASE rn.EntityName WHEN 'Document' THEN doc.docDesc END AS DocumentDescription
	, CASE rn.EntityName WHEN 'Document' THEN doc.documentExtension END AS DocumentExtension
	, CASE rn.EntityName WHEN 'Document' THEN doc.usrFullName END AS Author
	, CASE rn.EntityName WHEN 'Appointment' THEN app.appDesc END AS appDesc
	, CASE rn.EntityName WHEN 'Appointment' THEN app.appointmentType END AS AppointmentType
	, CASE rn.EntityName WHEN 'Appointment' THEN app.appLocation END AS AppointmentLocation
	, CASE rn.EntityName WHEN 'Task' THEN tsk.tskDesc END AS TaskDescription
	, CASE rn.EntityName WHEN 'Task' THEN tsk.taskType END AS taskType
	, @Total AS Total
FROM RN rn
	LEFT OUTER JOIN dbo.Precedent prec ON prec.mattersphereid = rn.mattersphereid AND rn.EntityName = 'Precedent'
	LEFT OUTER JOIN dbo.Contact cont ON cont.mattersphereid = rn.mattersphereid AND rn.EntityName = 'Contact'
	LEFT OUTER JOIN dbo.Client cl ON cl.mattersphereid = rn.mattersphereid AND rn.EntityName = 'Client'
	LEFT OUTER JOIN dbo.[File] f ON f.mattersphereid = rn.mattersphereid AND rn.EntityName = 'File'
	LEFT OUTER JOIN dbo.Associate assoc ON assoc.mattersphereid = rn.mattersphereid AND rn.EntityName = 'Associate'
	LEFT OUTER JOIN dbo.Document doc ON doc.mattersphereid = rn.mattersphereid AND rn.EntityName = 'Document'
	LEFT OUTER JOIN dbo.Appointment app ON app.mattersphereid = rn.mattersphereid AND rn.EntityName = 'Appointment'
	LEFT OUTER JOIN dbo.Task tsk ON tsk.mattersphereid = rn.mattersphereid AND rn.EntityName = 'Task'
WHERE rn.id > @OFFSET
ORDER BY rn.id


INSERT INTO #FacetRes (FacetField, cdCode, FacetValue, Cnt)
SELECT 'objecttype' 
	, 'MCTYPE'
	, UPPER(EntityName)
	, COUNT(*)
FROM @ResId
GROUP BY EntityName

SELECT FacetField
	, cdCode
	, FacetValue
	, SUM(Cnt) AS Cnt
FROM #FacetRes
GROUP BY FacetField, cdCode, FacetValue
