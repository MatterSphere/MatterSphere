"C:\Program Files (x86)\Microsoft SQL Server\140\DAC\bin\SqlPackage.exe" /Action:Publish ^
	/SourceFile:"{ENTERPATH}\MatterSphereDB.dacpac" ^
	/Profile:"{ENTERPATH}\MSIndex.publish.xml" ^
	/TargetConnectionString:"Data Source={SQLSERVER};Initial Catalog={DATABASE};Persist Security Info=False;Integrated Security=true;" ^
	/p:BlockOnPossibleDataLoss=FALSE  /p:DropObjectsNotInSource=FALSE ^
	/p:DropPermissionsNotInSource=FALSE  /p:DropRoleMembersNotInSource=FALSE
	
"C:\Program Files (x86)\Microsoft SQL Server\140\DAC\bin\SqlPackage.exe" /Action:Publish ^
	/SourceFile:"{ENTERPATH}\CentralDataSource.dacpac" ^
	/Profile:"{ENTERPATH}\CDSIndex.publish.xml" ^
	/TargetConnectionString:"Data Source={CDS_SQLSERVER};Initial Catalog={CDS_DATABASE};Persist Security Info=False;Integrated Security=true;"

PAUSE