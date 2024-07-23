-- Additions to OMS Import for 3E Integration
-- Run this after running the base OMSImport scripts

-- Correct database
USE OMSIMPORT

-- New Columns onto staging tables
ALTER TABLE DBO.CLIENTDETAILS ADD ClientIndex int NOT NULL
ALTER TABLE DBO.CLIENTDETAILS ADD EntIndex int NOT NULL
ALTER TABLE DBO.FILEDETAILS ADD MattIndex int NOT NULL
ALTER TABLE DBO.CLIENTDETAILS ADD feeUsrID int NULL
ALTER TABLE DBO.CLIENTDETAILS ADD brID int NULL

-- New entries in the Defaults table
INSERT INTO DBO.DEFAULTS VALUES ('CLBRANCH', 1, 'Client Default Branch')