-- Creates the Enterprise Integration Admin Kit Icons
-- CM: 2012-07-26

exec sprAddMenuItem 'AMUENTERPRISE', 'AMUSYSCONFIG', 10, 'Elite Enterprise', null, 0, 0, 200, null
exec sprAddMenuItem 'AMUENTEXCCLI', 'AMUENTERPRISE', 1,'%CLIENT% Export Exceptions', 'fdSCHEXCENTCLI', 0, 0, 0, null
exec sprAddMenuItem 'AMUENTEXCFIL', 'AMUENTERPRISE', 2,'%FILE% Export Exceptions', 'fdSCHEXCENTFIL', 0, 0, 1, null
exec sprAddMenuItem 'AMUENTEXCTIM', 'AMUENTERPRISE', 37,'Time Export Exceptions', 'fdSCHEXCENTTIM', 0, 0, 2, null
exec sprAddMenuItem 'AMUENTFEEMAP', 'AMUENTERPRISE', 7,'%FEEEARNER% Mapping', 'fdSCHENTFEEMAP', 0, 0, 3, null