
namespace FWBS.OMS.WFRuntime.Context
{
	/// <summary>
	/// This class defined the string constants for the argument key names that will passed to a server-side workflow
	/// with values extracted from the IContext passed.
	/// </summary>
	public sealed class ArgumentNames
	{
		public const string AssociateId = "ASSOCIATEID";
		public const string ClientId = "CLIENTID";
		public const string ContactId = "CONTACTID";
		public const string DocumentId = "DOCUMENTID";
		public const string DocumentVersion = "DOCUMENTVERSION";
		public const string FeeEarnerId = "FEEEARNERID";
		public const string FileId = "FILEID";
		public const string PrecedentId = "PRECEDENTID";
		public const string UserId = "USERID";
		public const string BranchId = "BRANCHID";

		public const string TokenPrefix = "MS_";
		public const string TokenInvokingUserId = TokenPrefix + "Invoke_" + UserId;
		public const string TokenAssociateId = TokenPrefix + AssociateId;
		public const string TokenClientId = TokenPrefix + ClientId;
		public const string TokenContactId = TokenPrefix + ContactId;
		public const string TokenDocumentId = TokenPrefix + DocumentId;
		public const string TokenDocumentVersion = TokenPrefix + DocumentVersion;
		public const string TokenFeeEarnerId = TokenPrefix + FeeEarnerId;
		public const string TokenFileId = TokenPrefix + FileId;
		public const string TokenPrecedentId = TokenPrefix + PrecedentId;
		public const string TokenUserId = TokenPrefix + UserId;
		public const string TokenBranchId = TokenPrefix + BranchId;
	}
}
