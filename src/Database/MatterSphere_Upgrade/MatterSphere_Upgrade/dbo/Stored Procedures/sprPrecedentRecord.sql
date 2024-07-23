CREATE  PROCEDURE [dbo].[sprPrecedentRecord]  (@PRECID bigint, @UI uUICultureInfo = '{default}') AS
-- PRECEDENT
SELECT         PREC.*, SP.spType AS SPType, DIR.dirPath AS precLivePath
FROM            dbPrecedents PREC LEFT OUTER JOIN
                      dbStorageProvider SP ON PREC.PrecLocation = SP.spID LEFT OUTER JOIN
                      dbDirectory DIR ON PREC.PrecDirID = DIR.dirID
where             PREC.precid = @PRECID

--MULTIPREC
select * from dbprecedentmulti pm
inner join dbPrecedents p
on p.precID = pm.multiChildID
where pm.multiMasterID = @PRECID
and (p.PrecLibrary is null or p.PrecLibrary <> 'ARCHIVE')
ORDER by multiorder
