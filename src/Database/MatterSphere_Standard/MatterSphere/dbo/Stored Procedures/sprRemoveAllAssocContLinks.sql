

CREATE PROCEDURE [dbo].[sprRemoveAllAssocContLinks]
(@contid bigint, @addid bigint = null, @tel nvarchar(25) = null, @fax nvarchar(25) = null, @mob nvarchar(25) = null, @email nvarchar(255) = null, @UI uUICultureInfo = null, @associd bigint = null)
as
if not @contid is null
begin
	if not @addid is null
	begin
	update dbassociates set assocdefaultaddid = null
	where contid = @contid and associd = COALESCE(@associd,associd)
		and (
			assocdefaultaddid = @addid
		)
	end
	else if not @email is null
	begin
	update dbassociates set assocemail = null
	where contid = @contid and associd = COALESCE(@associd,associd)
		and (
			assocemail = @email
		)
	end
	else if not @tel is null
	begin
	update dbassociates set assocddi = null
	where contid = @contid and associd = COALESCE(@associd,associd)
		and (
			assocddi = @tel
		)
	end
	else if not @fax is null
	begin
	update dbassociates set assocfax = null
	where contid = @contid and associd = COALESCE(@associd,associd)
		and (
			assocfax = @fax
		)
	end
	else if not @mob is null
	begin
	update dbassociates set assocmobile = null
	where contid = @contid and associd = COALESCE(@associd,associd)
		and (
			assocmobile = @mob
		)
	end
end
else
select 1 as ID, 'The is Delete Assococite Link Contacts for Email, Fac, Mobile, Address' as Description

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRemoveAllAssocContLinks] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRemoveAllAssocContLinks] TO [OMSAdminRole]
    AS [dbo];

