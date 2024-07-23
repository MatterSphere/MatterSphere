exec fwbsaddcolumn @tablename = 'dbPDFBundleHeader' , @columnName = 'bundleStatus' , @columnDesc = 'TINYINT NULL'
exec fwbsaddcolumn @tablename = 'dbPDFBundleHeader' , @columnName = 'bundleLog' , @columnDesc = 'NVARCHAR (MAX) NULL'
