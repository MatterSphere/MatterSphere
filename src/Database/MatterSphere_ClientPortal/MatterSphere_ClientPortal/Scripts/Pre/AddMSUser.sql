﻿IF NOT EXISTS ( SELECT * FROM DBUSER WHERE USRADID = N'$(UserName)' )
BEGIN
	
	INSERT INTO DBUSER ( USRINITS , USRALIAS , USRADID , USRFULLNAME , USRWORKSFOR , USRTYPE , BRID , USRWELCOMEWIZARD , USRUICULTUREINFO  , USRCURISOCODE , CREATED , CREATEDBY )
	VALUES ( 'MS_API_SVC' , 'MS_API_SVC' , '$(UserName)' , 'MatterSphere API Service Account' , -1 , 'SERVICE' , ( SELECT BRID FROM DBREGINFO ) , 0 , 'en-gb' , 'GBP' , GETUTCDATE () , -1 )
END 
GO
