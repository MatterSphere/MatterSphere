
--UPDATE TREE NODE TEXT FOR OBJECT TYPE CONFIGURATION & TYPE-WIDE CONFIGURATION ADMIN KIT TREE NODES

UPDATE DBCODELOOKUP SET CDDESC = REPLACE(CDDESC, 'MS Type-wide Configuration', 'Type-wide Configuration') WHERE CDTYPE = 'ADMINMENU' AND CDCODE = 'AMUTWCONFIG'

UPDATE DBCODELOOKUP SET CDDESC = REPLACE(CDDESC, 'MS Type-wide Configuration', 'Type-wide Configuration') WHERE CDTYPE = 'ADMTVNODES' AND CDCODE = 'OTCTWCONFIG'

UPDATE DBCODELOOKUP SET CDDESC = REPLACE(CDDESC, 'MS Object Type Configuration', 'Object Type Configuration') WHERE CDTYPE = 'ADMINMENU' AND CDCODE = 'AMUOTC'

