CREATE PROCEDURE [dbo].[fdsprsearchContact]
 @POSTCODE nvarchar(20)=null, @CONTNAME nvarchar(MAX)=null
 as

-- declare  @POSTCODE nvarchar(20)='NN13 3RT'

Begin
   --look for full postcode search first

   DECLARE @ResultsTable TABLE
     (Imagep int,contid bigint,contname nvarchar(160),ConcatAddress nvarchar(max),postcode nvarchar(20))


	 INSERT INTO @ResultsTable
	 select * from 
	(SELECT
		3 as Image
		,CONT.contID
		,CONT.contName	
		,dbo.GetAddress( coalesce( ( select top 1 contaddid from dbcontactaddresses where contid = CONT.contid and contactive = 1 and contcode = 'PRINCIPLE' ), contdefaultaddress)
						, ',',null) as ConcatAddress	
		,(coalesce((select top 1 a.addPostCode 
					from dbcontactaddresses ca
					inner join dbaddress a on a.addid = ca.contaddid
					where contid = CONT.contid and contactive = 1 and contcode = 'PRINCIPLE'
				  ),
				 (
					select top 1 a.addPostCode 
					from dbcontactaddresses ca
					inner join dbaddress a on a.addid = CONT.contdefaultaddress
					where contid = CONT.contid and contactive = 1 
				  )	) )  as postcode
    
		FROM
			dbContact CONT) A 
		WHERE
		   ( (A.postcode like '%' +  @POSTCODE + '%')) 
   


   
   if @@ROWCOUNT =0 --it can't find any rows from above query 

    begin
	  INSERT INTO @ResultsTable
	   select * from 
		(SELECT
			3 as Image
			,CONT.contID
			,CONT.contName	
			,dbo.GetAddress( coalesce( ( select top 1 contaddid from dbcontactaddresses where contid = CONT.contid and contactive = 1 and contcode = 'PRINCIPLE' ), contdefaultaddress)
							, ',',null) as ConcatAddress	
			,(coalesce((select top 1 a.addPostCode 
						from dbcontactaddresses ca
						inner join dbaddress a on a.addid = ca.contaddid
						where contid = CONT.contid and contactive = 1 and contcode = 'PRINCIPLE'
					  ),
					 (
						select top 1 a.addPostCode 
						from dbcontactaddresses ca
						inner join dbaddress a on a.addid = CONT.contdefaultaddress
						where contid = CONT.contid and contactive = 1 
					  )	) )  as postcode
    
		FROM
			dbContact CONT) B 
		WHERE
		  (B.CONTNAME LIKE '%' + @CONTNAME + '%' ) OR		  
		  (LEFT(B.postcode,3) like '%' +  LEFT(@POSTCODE,3) + '%')

	end    


	SELECT * FROM @ResultsTable
   
  end 
