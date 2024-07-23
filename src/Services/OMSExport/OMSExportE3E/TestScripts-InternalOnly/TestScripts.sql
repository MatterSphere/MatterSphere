USE MSELITE
SET NOCOUNT ON
--THIS TEST SCRIPT IS SUBJECT TO CHANGES IN DATA AS WELL AS STRUCTURE
--TESTS WRITTEN FOR LOCAL TEST MATTERSPHERE DATABASE - UK-3E-SUP07.MSELITE

--CHECK EXPORT XML FOR CLID 1
DECLARE @EXPECTED_GetE3EClientExportXML1  NVARCHAR ( MAX ) = '<Client_Srv xmlns="http://elite.com/schemas/transaction/process/write/Client_Srv" >
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/Client">
   <Add>
    <Client>
     <Attributes>
      <Entity>174</Entity>
      <Number>F1</Number>
      <DisplayName>Mr A Person</DisplayName>
	  <CliStatusType>Pending</CliStatusType>
	  <OpenDate>May 10 2013</OpenDate>
      <OpenTkpr>4</OpenTkpr>
      <IsPayor>1</IsPayor>
      <InvoiceSite>%INVOICESITE%</InvoiceSite>
	  <Narrative></Narrative>
	  <VATRegistration>MISSING</VATRegistration>
	  <LoadSource>MatterSphere</LoadSource>
	  <LoadNumber>1</LoadNumber>
	  <PayorTaxNum>MISSING</PayorTaxNum>
     </Attributes>
     <Children>
      <CliDate>
       <Edit>
        <CliDate Position="0">
         <Attributes>
	      <BillTkpr>4</BillTkpr>
	      <RspTkpr>4</RspTkpr>
	      <SpvTkpr>4</SpvTkpr>
	      <Office>101</Office>
         </Attributes>
        </CliDate>
       </Edit>	
      </CliDate>			
     </Children>		
    </Client>
   </Add>
  </Initialize>
 </Client_Srv>'
IF DBO.GetE3EClientExportXML ( 1 ) = @EXPECTED_GetE3EClientExportXML1 
BEGIN PRINT 'OK : @EXPECTED_GetE3EClientExportXML1' END ELSE BEGIN PRINT 'FAILED : @EXPECTED_GetE3EClientExportXML1' END 

--CHECK UPDATE XML FOR CLID 1


DECLARE @EXPECTED_GetE3EClientUpdateXML1  NVARCHAR ( MAX ) = '<Client_Srv xmlns="http://elite.com/schemas/transaction/process/write/Client_Srv" >
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/Client">
   <Edit>
    <Client KeyValue = "66">
     <Attributes>
      <DisplayName>Mr A Person</DisplayName>
      <Narrative></Narrative>
	  <VATRegistration>MISSING</VATRegistration>
	  <PayorTaxNum>MISSING</PayorTaxNum>
     </Attributes>
    </Client>
   </Edit>
  </Initialize>
 </Client_Srv>'
IF DBO.GetE3EClientUpdateXML ( 1 ) = @EXPECTED_GetE3EClientUpdateXML1 
BEGIN PRINT 'OK : @@EXPECTED_GetE3EClientUpdateXML1' END ELSE BEGIN PRINT 'FAILED : @@EXPECTED_GetE3EClientUpdateXML1' END 

DECLARE @EXPECTED_GetE3EEntityPersonExportXML1 NVARCHAR ( MAX ) = '<EntityPerson_Srv xmlns="http://elite.com/schemas/transaction/process/write/EntityPerson_Srv" >
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/EntityPerson">
   <Add>
    <EntityPerson>
     <Attributes>
      <Comment></Comment>
	  <FirstName>Alan</FirstName>
      <MiddleName></MiddleName>
      <LastName>Person</LastName>
      <BirthDate>1974-04-27T00:00:00</BirthDate>
      <Gender></Gender>
      <SSNumber></SSNumber>
	  <LoadSource>MatterSphere</LoadSource>
	  <LoadNumber>1</LoadNumber>
     </Attributes>
     <Children>
      <Relate>
       <Edit>
        <Relate Position="0">
         <Children>
          <Site>
           <Add>
            <Site>
             <Attributes>
              <Description>Default Address - MatterSphere</Description>
              <SiteType>Billing</SiteType>
              <Street>BillMeHere</Street>
              <Additional1></Additional1>
              <Additional2></Additional2>
              <City>BillTown</City>
              <County>BillShire</County>
              <State></State>
              <ZipCode>B177</ZipCode>
              <Country>GB</Country>
			  <IsDefault>1</IsDefault>
             </Attributes>
            <Children>
              <Site_Phone>
               <Add>
                <Site_Phone>
                 <Attributes>
                  <Description>Default Number</Description>
                  <Number>01604 111111</Number>
                 </Attributes>
                </Site_Phone>
               </Add>
              </Site_Phone>
             </Children>
            </Site>
           </Add>
          </Site>
         </Children>
        </Relate>
       </Edit>
      </Relate>
     </Children>
    </EntityPerson>
   </Add>
  </Initialize>
 </EntityPerson_Srv>'
DECLARE @CONTEXTID_WAS INT
SELECT @CONTEXTID_WAS = CONTEXTID FROM DBCONTACT WHERE CONTID = 1
UPDATE DBCONTACT SET CONTEXTID = NULL WHERE CONTID = 1
IF DBO.GetE3EEntityPersonExportXML ( 1 ) = @EXPECTED_GetE3EEntityPersonExportXML1 
BEGIN PRINT 'OK : @EXPECTED_GetE3EEntityPersonExportXML' END ELSE BEGIN PRINT 'FAILED : @EXPECTED_GetE3EEntityPersonExportXML' END 
UPDATE DBCONTACT SET CONTEXTID = @CONTEXTID_WAS , CONTNEEDEXPORT = 0 WHERE CONTID = 1

DECLARE @EXPECTED_GetE3EEntityPersonUpdateXML1 NVARCHAR ( MAX ) = '<EntityPerson_Srv xmlns="http://elite.com/schemas/transaction/process/write/EntityPerson_Srv" >
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/EntityPerson">
   <Edit>
    <EntityPerson KeyValue = "174">
     <Attributes>
      <Comment></Comment>
      <FirstName>Alan</FirstName>
      <MiddleName></MiddleName>
      <LastName>Person</LastName>
      <BirthDate>1974-04-27T00:00:00</BirthDate>
      <Gender></Gender>
      <SSNumber></SSNumber>
     </Attributes>
	 <Children>
      <Relate>
       <Edit>
        <Relate Position="0">
         <Children>
          <Site>
           <Edit>
            <Site KeyValue="%SITEINDEX%">
             <Attributes>
              <Description>Default Address - MatterSphere</Description>
              <SiteType>Billing</SiteType>
              <Street>BillMeHere</Street>
              <Additional1></Additional1>
              <Additional2></Additional2>
              <City>BillTown</City>
              <County>BillShire</County>
              <State></State>
              <ZipCode>B177</ZipCode>
              <Country>GB</Country>
			  <IsDefault>1</IsDefault>
             </Attributes>
            </Site>
           </Edit>
          </Site>
         </Children>
        </Relate>
       </Edit>
      </Relate>
     </Children>
    </EntityPerson>
   </Edit>
  </Initialize>
 </EntityPerson_Srv>'
IF DBO.GetE3EEntityPersonUpdateXML ( 1 ) = @EXPECTED_GetE3EEntityPersonUpdateXML1 
BEGIN PRINT 'OK : @EXPECTED_GetE3EEntityPersonUpdateXML1' END ELSE BEGIN PRINT 'FAILED : @EXPECTED_GetE3EEntityPersonUpdateXML1' END 



DECLARE @EXPECTED_GetE3EEntityOrgExportXML2 NVARCHAR ( MAX ) = '<NxBizTalkEntityOrgLoad xmlns="http://elite.com/schemas/transaction/process/write/NxBizTalkEntityOrgLoad" >
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/EntityOrg">
   <Add>
    <EntityOrg>
     <Attributes>
      <Comment></Comment>
      <OrgName>Denby Corporation</OrgName>
	  <LoadSource>MatterSphere</LoadSource>
	  <LoadNumber>2</LoadNumber>
     </Attributes>
     <Children>
      <Relate>
       <Edit>
        <Relate Position="0">
         <Children>
          <Site>
           <Add>
            <Site>
             <Attributes>
              <Description>Default Address - MatterSphere</Description>
              <SiteType>Billing</SiteType>
              <Street>1 Low Road</Street>
              <Additional1></Additional1>
              <Additional2></Additional2>
              <City>Northampton</City>
              <County></County>
              <State></State>
              <ZipCode>NN1 1AB</ZipCode>
              <Country>GB</Country>
			  <IsDefault>1</IsDefault>
             </Attributes>
            <Children>
              <Site_Phone>
               <Add>
                <Site_Phone>
                 <Attributes>
                  <Description>Default Number</Description>
                  <Number>09876 54321</Number>
                 </Attributes>
                </Site_Phone>
               </Add>
              </Site_Phone>
             </Children>
            </Site>
           </Add>
          </Site>
         </Children>
        </Relate>
       </Edit>
      </Relate>
     </Children>
    </EntityOrg>
   </Add>
  </Initialize>
 </NxBizTalkEntityOrgLoad>'
DECLARE @CONTEXTID_ORG_WAS INT
SELECT @CONTEXTID_ORG_WAS = CONTEXTID FROM DBCONTACT WHERE CONTID = 2
UPDATE DBCONTACT SET CONTEXTID = NULL WHERE CONTID = 2
IF DBO.GetE3EEntityOrgExportXML ( 2 ) = @EXPECTED_GetE3EEntityOrgExportXML2
BEGIN PRINT 'OK : @EXPECTED_GetE3EEntityPersonExportXML' END ELSE BEGIN PRINT 'FAILED : @EXPECTED_GetE3EEntityPersonExportXML' END 
UPDATE DBCONTACT SET CONTEXTID = @CONTEXTID_ORG_WAS , CONTNEEDEXPORT = 0 WHERE CONTID = 2


DECLARE @EXPECTED_GetE3EEntityOrgUpdateXML2 NVARCHAR ( MAX ) = '<NxBizTalkEntityOrgLoad xmlns="http://elite.com/schemas/transaction/process/write/NxBizTalkEntityOrgLoad" >
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/EntityOrg">
   <Edit>
    <EntityOrg KeyValue = "25">
     <Attributes>
      <Comment></Comment>
      <OrgName>Denby Corporation</OrgName>
     </Attributes>
     <Children>
      <Relate>
       <Edit>
        <Relate Position="0">
         <Children>
          <Site>
           <Edit>
            <Site KeyValue="%SITEINDEX%">
             <Attributes>
			  <OrgName>Denby Corporation</OrgName>
              <Description>Default Address - MatterSphere</Description>
              <SiteType>Billing</SiteType>
              <Street>1 Low Road</Street>
              <Additional1></Additional1>
              <Additional2></Additional2>
              <City>Northampton</City>
              <County></County>
              <State></State>
              <ZipCode>NN1 1AB</ZipCode>
              <Country>GB</Country>
			  <IsDefault>1</IsDefault>
             </Attributes>
            </Site>
           </Edit>
          </Site>
         </Children>
        </Relate>
       </Edit>
      </Relate>
     </Children>
    </EntityOrg>
   </Edit>
  </Initialize>
 </NxBizTalkEntityOrgLoad>'
IF DBO.GetE3EEntityOrgUpdateXML ( 2 ) = @EXPECTED_GetE3EEntityOrgUpdateXML2 
BEGIN PRINT 'OK : @EXPECTED_GetE3EEntityOrgUpdateXML2' END ELSE BEGIN PRINT 'FAILED : @EXPECTED_GetE3EEntityOrgUpdateXML2' END 

DECLARE @EXPECTED_GetE3EMatterExportXML1 NVARCHAR ( MAX ) = '<Matter_Srv xmlns="http://elite.com/schemas/transaction/process/write/Matter_Srv" >
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/Matter">
   <Add>
    <Matter>
     <Attributes>
      <Number>1002-1</Number>
	  <Client>2</Client>
	  <DisplayName>General Employment</DisplayName>
	  <MattStatus>Prospective</MattStatus>
	  <MattType>Admin</MattType>
	  <OpenDate>May 10 2013</OpenDate>
	  <Narrative></Narrative>
      <OpenTkpr>4</OpenTkpr>
	  <LoadSource>MatterSphere</LoadSource>
	  <LoadNumber>1</LoadNumber>
     </Attributes>
     <Children>
	  <MattDate>
       <Edit>
        <MattDate Position="0">
         <Attributes>
          <Office>101</Office>
          <Department>000</Department>
          <Section>000</Section>
          <PracticeGroup>Default</PracticeGroup>
          <BillTkpr>4</BillTkpr>
          <RspTkpr>4</RspTkpr>
          <SpvTkpr>4</SpvTkpr>
          <Arrangement>Hourly</Arrangement>
         </Attributes>
        </MattDate>
       </Edit>
      </MattDate> 
      <MattRate>
       <Edit>
        <MattRate Position="0">
         <Attributes>
          <Rate>Tkpr_Std</Rate>
         </Attributes>
        </MattRate>
       </Edit>
     </MattRate>
     </Children>
    </Matter>
   </Add>
  </Initialize>
 </Matter_Srv>'
IF DBO.GetE3EMatterExportXML ( 1 ) = @EXPECTED_GetE3EMatterExportXML1 
BEGIN PRINT 'OK : @EXPECTED_GetE3EMatterExportXML1' END ELSE BEGIN PRINT 'FAILED : @EXPECTED_GetE3EMatterExportXML1' END 

DECLARE @EXPECTED_GetE3EMatterUpdateXML1 NVARCHAR ( MAX ) = '<Matter_Srv xmlns="http://elite.com/schemas/transaction/process/write/Matter_Srv" >
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/Matter">
   <Edit>
    <Matter KeyValue = "3">
     <Attributes>
        <DisplayName>General Employment</DisplayName>
        <MattType>Admin</MattType>
        <Narrative></Narrative>
     </Attributes>
     <Children>
     </Children>
    </Matter>
   </Edit>
  </Initialize>
</Matter_Srv>'
IF DBO.GetE3EMatterUpdateXML ( 1 ) = @EXPECTED_GetE3EMatterUpdateXML1 
BEGIN PRINT 'OK : @EXPECTED_GetE3EMatterUpdateXML1' END ELSE BEGIN PRINT 'FAILED : @EXPECTED_GetE3EMatterUpdateXML1' END 


