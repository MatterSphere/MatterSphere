Print 'NewInstallScripts\Default_Language.sql'


-- add default languages
IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'af' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'af' , 'Afrikaans' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar' , 'Arabic' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-ae' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-ae' , 'Arabic (U.A.E)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-bh' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-bh' , 'Arabic (Bahrain)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-dz' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-dz' , 'Arabic (Algeria)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-eg' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-eg' , 'Arabic (Egypt)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-iq' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-iq' , 'Arabic (Iraq)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-jo' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-jo' , 'Arabic (Jordan)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-kw' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-kw' , 'Arabic (Kuwait)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-lb' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-lb' , 'Arabic (Lebanon)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-ly' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-ly' , 'Arabic (Libya)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-ma' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-ma' , 'Arabic (Morocco)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-om' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-om' , 'Arabic (Oman)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-qa' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-qa' , 'Arabic (Quatar)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-sa' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-sa' , 'Arabic (Saudi Arabia)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-sy' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-sy' , 'Arabic (Syria)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-tn' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-tn' , 'Arabic (Tunisia)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ar-ye' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ar-ye' , 'Arabic (Yemen)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'az' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'az' , 'Azeri (Latin / Cyrillic)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'be' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'be' , 'Belarusian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'bg' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'bg' , 'Bulgarian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ca' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ca' , 'Catalan' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'cs' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'cs' , 'Czech' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'da' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'da' , 'Danish' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'de' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'de' , 'German (Standard)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'de-at' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'de-at' , 'German (Austria)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'de-ch' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'de-ch' , 'German (Switzerland)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'de-li' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'de-li' , 'German (Liechtenstein)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'de-lu' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'de-lu' , 'German (Luxembourg)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'el' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'el' , 'Greek' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en' , 'English' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-au' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-au' , 'English (Australia)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-bz' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-bz' , 'English (Belize)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-ca' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-ca' , 'English (Canada)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-cb' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-cb' , 'English (Caribbean)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-gb' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-gb' , 'English (Great Britain)' , 1 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-ie' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-ie' , 'English (Ireland)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-jm' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-jm' , 'English (Jamaica)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-nz' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-nz' , 'English (New Zealand)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-ph' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-ph' , 'English (Phillipines)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-tt' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-tt' , 'English (Trinidad)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-us' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-us' , 'English (United States)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'en-za' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'en-za' , 'English (South Africa)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es' , 'Spanish (Modern / Traditional)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-ar' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-ar' , 'Spanish (Argentina)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-bo' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-bo' , 'Spanish (Bolivia)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-cl' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-cl' , 'Spanish (Chile)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-co' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-co' , 'Spanish (Colombia)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-cr' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-cr' , 'Spanish (Costa Rica)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-do' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-do' , 'Spanish (Dominican Republic)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-ec' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-ec' , 'Spanish (Ecuador)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-gt' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-gt' , 'Spanish (Guatemala)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-hn' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-hn' , 'Spanish (Honduras)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-mx' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-mx' , 'Spanish (Mexico)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-ni' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-ni' , 'Spanish (Nicaragua)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-pa' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-pa' , 'Spanish (Panama)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-pe' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-pe' , 'Spanish (Peru)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-pr' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-pr' , 'Spanish (Puerto Rico)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-py' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-py' , 'Spanish (Paraguay)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-sv' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-sv' , 'Spanish (El Salvador)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-uy' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-uy' , 'Spanish (Uruguay)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'es-ve' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'es-ve' , 'Spanish (Venezuela)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'et' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'et' , 'Estonian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'eu' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'eu' , 'Basque' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'fa' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'fa' , 'Farsi' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'fi' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'fi' , 'Finnish' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'fo' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'fo' , 'Faeroese' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'fr' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'fr' , 'French (Standard)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'fr-be' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'fr-be' , 'French (Belgium)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'fr-ca' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'fr-ca' , 'French (Canada)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'fr-ch' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'fr-ch' , 'French (Switzerland)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'fr-lu' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'fr-lu' , 'French (Luxembourg)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'gd' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'gd' , 'Gaelic (Scotland)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'gd-ie' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'gd-ie' , 'Gaelic (Ireland)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'he' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'he' , 'Hebrew' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'hi' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'hi' , 'Hindi' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'hr' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'hr' , 'Croatian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ht' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ht' , 'Armenian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'hu' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'hu' , 'Hungarian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'id' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'id' , 'Indonesian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'in' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'in' , 'Indonesian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'is' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'is' , 'Iceland' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'it' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'it' , 'Italian (Standard)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'it-ch' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'it-ch' , 'Italian (Switzerland)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'iw' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'iw' , 'Hebrew' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ja' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ja' , 'Japanese' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ji' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ji' , 'Yiddish' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ko' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ko' , 'Korean (Johab)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'lt' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'lt' , 'Lithuanian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'lv' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'lv' , 'Latvian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'mk' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'mk' , 'FYRO Macedonian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'mr' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'mr' , 'Marathi' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ms' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ms' , 'Malaysian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ms-bn' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ms-bn' , 'Malaysian - Brunei' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ms-my' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ms-my' , 'Malaysian - Malaysia' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'mt' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'mt' , 'Maltese' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'nl' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'nl' , 'Dutch (Standard)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'nl-be' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'nl-be' , 'Dutch (Belgium)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'no' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'no' , 'Norwegian (Bokmal / Nynorsk)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'pl' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'pl' , 'Polish' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'pt' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'pt' , 'Portuguese (Portugal)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'pt-br' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'pt-br' , 'Portuguese (Brazil)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'rm' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'rm' , 'Rhaeto-Romanic' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ro' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ro' , 'Romanian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ro-mo' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ro-mo' , 'Romanian (Moldova)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ru' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ru' , 'Russian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ru-mo' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ru-mo' , 'Russian (Moldova)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sa' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sa' , 'Sanskrit' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sb' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sb' , 'Sorbian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sk' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sk' , 'Slovak' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sl' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sl' , 'Slovenian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sq' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sq' , 'Albanian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sr' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sr' , 'Serbian (Cyrillic / Latin)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sv' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sv' , 'Swedish' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sv-fi' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sv-fi' , 'Swedish (Finland)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sv-se' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sv-se' , 'Swedish (Sweden)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sw' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sw' , 'Swahili' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sx' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sx' , 'Sutu' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'sz' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'sz' , 'Sami (Lappish)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ta' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ta' , 'Tamil' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'th' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'th' , 'Thai' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'tn' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'tn' , 'Tswana / Setsuana' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'tr' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'tr' , 'Turkish' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ts' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ts' , 'Tsonga' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'tt' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'tt' , 'Tatar' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'uk' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'uk' , 'Ukranian' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'ur' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'ur' , 'Urdu' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'uz' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'uz' , 'Uzbek (Latin / Cyrillic)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 've' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 've' , 'Venda' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'vi' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'vi' , 'Vietnamese' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'xh' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'xh' , 'Xhosa' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'xn' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'xn' , 'Xhosa' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'zh' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'zh' , 'Chinese' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'zh-cn' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'zh-cn' , 'Chinese (China - PRC)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'zh-hk' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'zh-hk' , 'Chinese (Hong Kong SAR)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'zh-mo' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'zh-mo' , 'Chinese (Macau SAR)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'zh-sg' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'zh-sg' , 'Chinese (Singapore)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'zh-tw' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'zh-tw' , 'Chinese (Taiwan)' , 0 )
END
GO

IF NOT EXISTS ( SELECT langCode FROM dbo.dbLanguage WHERE langCode = 'zu' )
BEGIN
	INSERT dbo.dbLanguage ( langCode , langDesc , langSupported )
	VALUES ( 'zu' , 'Zulu' , 0 )
END
GO
