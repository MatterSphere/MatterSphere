/*DECLARE @CLRRunTime nvarchar(20)
SET @CLRRunTime = CONVERT(Nvarchar(20),SERVERPROPERTY('BuildCLRVersion'))
DECLARE @location nvarchar(2048)
SET @location = (SELECT TOP 1 spDATA FROM dbSpecificData S join dbRegInfo RI on RI.brid = S.brID WHERE spLookup = 'DirectoryServices.DLL_Location')
IF @location = '' or @location is null

BEGIN
IF charindex('64-bit',(SELECT 'Edition: '+Convert(nvarchar, ServerProperty('Edition')))) = 0  
SET @location ='C:\Windows\Microsoft.NET\Framework\'+ @CLRRunTime +'\System.DirectoryServices.dll' --32 Bit default location
ELSE SET @location ='C:\Windows\Microsoft.NET\Framework64\'+ @CLRRunTime +'\System.DirectoryServices.dll' --64 Bit default location
END

ALTER ASSEMBLY [DirectoryServices]
FROM @location
WITH PERMISSION_SET = UNSAFE
GO*/