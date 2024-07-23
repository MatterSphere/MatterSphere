"C:\Program Files[ (x86)]\Microsoft SQL Server\{VersionNUMBER}\DAC\bin\SqlPackage.exe" /Action:Publish ^
  /SourceFile:"{ENTERPATH}\MatterSphere_Auditing.dacpac" ^
  /Profile:"{ENTERPATH}\MatterSphere_Auditing.publish.xml" ^
   /TargetConnectionString:"Data Source={SQLSERVER};Initial Catalog={DATABASE};Persist Security Info=False;Integrated Security=true;" ^
   /p:BlockOnPossibleDataLoss=FALSE   /p:DropObjectsNotInSource=FALSE ^
   /p:DropPermissionsNotInSource=FALSE  /p:DropRoleMembersNotInSource=FALSE

PAUSE