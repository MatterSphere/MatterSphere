IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'search.FormatUserAccessList') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION search.FormatUserAccessList

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'search.GETAvailableCrawls') AND type in (N'P', N'PC'))
DROP PROCEDURE search.GETAvailableCrawls

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'search.GETData') AND type in (N'P', N'PC'))
DROP PROCEDURE search.GETData

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'search.GetTotalToProcess') AND type in (N'P', N'PC'))
DROP PROCEDURE search.GetTotalToProcess

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'search.Crawl') AND type in (N'U'))
DROP TABLE search.Crawl
