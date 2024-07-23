﻿IF EXISTS(SELECT 1 FROM dbo.dbContactIndividual WHERE contDOB <> CONVERT(VARCHAR(10),contDOB, 121) OR contDOD <> CONVERT(VARCHAR(10),contDOD, 121))
	UPDATE dbo.dbContactIndividual
	SET contDOB = DATEADD(hh, CASE WHEN DATEDIFF(hh, CONVERT(VARCHAR(10),contDOB, 121), contDOB ) > 12 THEN 24 - DATEDIFF(hh, CONVERT(VARCHAR(10),contDOB, 121), contDOB ) ELSE - DATEDIFF(hh, CONVERT(VARCHAR(10),contDOB, 121), contDOB ) END, contDOB)
		, contDOD = DATEADD(hh, CASE WHEN DATEDIFF(hh, CONVERT(VARCHAR(10),contDOD, 121), contDOD ) > 12 THEN 24 - DATEDIFF(hh, CONVERT(VARCHAR(10),contDOD, 121), contDOD ) ELSE - DATEDIFF(hh, CONVERT(VARCHAR(10),contDOD, 121), contDOD ) END, contDOD)
	WHERE contDOB <> CONVERT(VARCHAR(10),contDOB, 121)
		OR contDOD <> CONVERT(VARCHAR(10),contDOD, 121)