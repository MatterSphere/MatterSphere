

CREATE PROCEDURE [dbo].[srepContactSecForm]
	@contID bigint ,
	@uI uUICultureInfo = '{default}'
AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT 
	contName,
	CONT.Created,
	dbo.GetAddress(CONT.contdefaultaddress,', ',@UI) as AddressLine,
	dbo.GetUser(CONT.CreatedBy,'USRFULLNAME') as CreatedBy,
	IMG1.text as text1,
	IMG1.image as image1,
	IMG2.text as text2,
	IMG2.image as image2,
	IMG3.text as text3,
	IMG3.image as image3,
	IMG4.image as image4,
	IMG4.text as text4,
	dbo.getCodeLookupDesc('SECQUESTION',SEC.question,@UI) as SecQuestion,
	PassPhrase
from 
	dbContact CONT 
inner join 
	dbcontactsecurity SEC on SEC.contid = CONT.contid
left join 
	dbImages IMG1 on IMG1.ID = SEC.imageID1
left join 
	dbImages IMG2 on IMG2.ID = SEC.imageID2
left join 
	dbImages IMG3 on IMG3.ID = SEC.imageID3
left join 
	dbImages IMG4 on IMG4.ID = SEC.imageID4
where 
	CONT.contid = @contID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContactSecForm] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepContactSecForm] TO [OMSAdminRole]
    AS [dbo];

