﻿UPDATE CT
SET typeActive = 0
FROM dbo.dbContactType CT
WHERE CT.typeCode IN ('3RDPARTY', 'BARRISTER','CLIENT', 'COURT', 'EAGENT', 'EXPERT', 'FBROKER', 'FINANCIAL', 'INSBROKER', 'INSCO', 'INSCOMASS', 'IREV', 'LANDREG', 'LBROKER', 'LEGOMORG', 'LOCALAUTH', 'LOCAUTAGY', 'REMOTE', 'SHERIFF', 'SOLICITOR', 'SUBCLIENT', 'TEMPLATE', 'TMPCORP', 'WATAUTH')
	AND CT.typeActive = 1
	AND NOT EXISTS(SELECT * FROM config.dbContact WHERE contTypeCode = CT.typeCode)