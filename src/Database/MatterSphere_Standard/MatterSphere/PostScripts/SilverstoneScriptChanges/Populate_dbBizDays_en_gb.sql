Print 'SilverstoneScriptChanges\Populate_dbBizDays_en_gb.sql'


SET NOCOUNT ON

IF NOT EXISTS ( SELECT * FROM dbo.dbBizDay WHERE bizCultureInfo = 'en-gb')
BEGIN
DECLARE @dt SMALLDATETIME; 
SET @dt = '20090101'; 
WHILE @dt <= '20201231' 
BEGIN 
    INSERT dbo.dbBizDay(bizDate , bizCultureInfo) SELECT @dt , 'en-gb'
    SET @dt = @dt + 1; 
END 
--GO

-- now reset weekends back to 0 
UPDATE dbo.dbBizDay SET bizIsWorkDay = 0 WHERE bizIsWeekday = 0 
--GO

-- update known English holidays, this needs confirmation.
UPDATE dbo.dbBizDay SET bizIsWorkDay = 0 WHERE bizIsWorkDay = 1 AND bizCultureInfo = 'en-gb' AND bizDate IN 
    ( 
		-- 2009 
        '20090101', -- New Years 
        '20091225', -- Christmas 
        '20091228',-- Boxing Day 
        '20090410', -- Good Friday
        '20090413', -- Easter Monday
        -- 2010
        '20100101', -- New Years 
        '20101227', -- Christmas 
        '20101228', -- Boxing Day 
        '20100402', -- Good Friday
        '20100405', -- Easter Monday
        -- 2011
        '20110103', -- New Years 
        '20111226', -- Christmas 
        '20111227', -- Boxing Day 
        '20110422', -- Good Friday
        '20110425', -- Easter Monday
         -- 2012
        '20120102', -- New Years 
        '20121225', -- Christmas 
        '20121226', -- Boxing Day 
        '20120406', -- Good Friday
        '20120409', -- Easter Monday
         -- 2013
        '20130101', -- New Years 
        '20131225', -- Christmas 
        '20131226', -- Boxing Day 
        '20130329', -- Good Friday
        '20130401', -- Easter Monday
         -- 2014
        '20140101', -- New Years 
        '20141225', -- Christmas 
        '20141226', -- Boxing Day 
        '20140418', -- Good Friday
        '20140421', -- Easter Monday
         -- 2015
        '20150101', -- New Years 
        '20151225', -- Christmas 
        '20151228', -- Boxing Day 
        '20150403', -- Good Friday
        '20150406', -- Easter Monday
         -- 2016
        '20160101', -- New Years 
        '20161226', -- Christmas 
        '20161227', -- Boxing Day 
        '20160425', -- Good Friday
        '20160428', -- Easter Monday
         -- 2017
        '20170102', -- New Years 
        '20171225', -- Christmas 
        '20171226', -- Boxing Day 
        '20170414', -- Good Friday
        '20170417', -- Easter Monday
         -- 2018
        '20180101', -- New Years 
        '20181225', -- Christmas 
        '20181226', -- Boxing Day
        '20180331', -- Good Friday
        '20180402', -- Easter Monday 
        -- 2019
        '20190101', -- New Years 
        '20191225', -- Christmas 
        '20191226', -- Boxing Day 
        '20190419', -- Good Friday
        '20190422', -- Easter Monday
        -- 2020
        '20200101', -- New Years 
        '20201225', -- Christmas 
        '20201226', -- Boxing Day 
        '20200410', -- Good Friday
        '20200413' -- Easter Monday
    )
 -- GO

-- now include May day bank holiday
UPDATE dbBizDay
SET bizIsWorkDay = 0 WHERE bizCultureInfo = 'en-gb' AND bizDate IN 
	(
	SELECT min(bizDate) FROM dbBizDay WHERE Datepart(dw, bizDate) = 2 AND Datepart(month , bizdate) = 4 AND bizCultureInfo = 'en-gb'
	group by Datepart(year , bizdate) 
	) 
--GO

-- now include May bank holiday
UPDATE dbBizDay
SET bizIsWorkDay = 0 WHERE bizCultureInfo = 'en-gb' AND bizDate IN 
	(
	SELECT max(bizDate) FROM dbBizDay WHERE Datepart(dw, bizDate) = 2 AND Datepart(month , bizdate) = 4 AND bizCultureInfo = 'en-gb'
	group by Datepart(year , bizdate) 
	) 
--GO
	
	-- now include August day bank holiday
UPDATE dbBizDay
SET bizIsWorkDay = 0 WHERE bizCultureInfo = 'en-gb' AND bizDate IN 
	(
	SELECT max(bizDate) FROM dbBizDay WHERE Datepart(dw, bizDate) = 2 AND Datepart(month , bizdate) = 8 AND bizCultureInfo = 'en-gb'
	group by Datepart(year , bizdate) 
	) 
--GO

END
GO
