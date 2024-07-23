using System;
using Fwbs.Framework.Interop;

namespace Redemption
{
    public sealed class RedemptionFactory : COMFactory
    {
        #region Static

        private static readonly RedemptionFactory def = new RedemptionFactory();

        public static RedemptionFactory Default
        {
            get
            { 
                return def;
            }
        }

        #endregion

        #region Constants

        public const string Name64 = "Redemption64.dll";
        public const string Name32 = "Redemption.dll";

        #endregion

        #region Constructors

        public RedemptionFactory()
            : base(CreateFile())
        {
        }

        public RedemptionFactory(DllInfo file)
            : base(file)
        {
        }

        private static DllInfo CreateFile()
        {
            return new DllInfo(Name32, Name64);
        }

        #endregion

        #region Factory Methods

        //The only creatable RDO object - RDOSession
        public RDOSession CreateRDOSession()
        {
            return (RDOSession)Create(new Guid("29AB7A12-B531-450E-8F7A-EA94C2F3C05F")) ?? new RDOSession();
        }

        //Safe*Item objects
        public SafeMailItem CreateSafeMailItem()
        {
            return (SafeMailItem)Create(new Guid("741BEEFD-AEC0-4AFF-84AF-4F61D15F5526")) ?? new SafeMailItem();
        }

        public SafeContactItem CreateSafeContactItem()
        {
            return (SafeContactItem)Create(new Guid("4FD5C4D3-6C15-4EA0-9EB9-EEE8FC74A91B")) ?? new SafeContactItem();
        }

        public SafeAppointmentItem CreateSafeAppointmentItem()
        {
            return (SafeAppointmentItem)Create(new Guid("620D55B0-F2FB-464E-A278-B4308DB1DB2B")) ?? new SafeAppointmentItem();
        }

        public SafeTaskItem CreateSafeTaskItem()
        {
            return (SafeTaskItem)Create(new Guid("7A41359E-0407-470F-B3F7-7C6A0F7C449A")) ?? new SafeTaskItem();
        }

        public SafeJournalItem CreateSafeJournalItem()
        {
            return (SafeJournalItem)Create(new Guid("C5AA36A1-8BD1-47E0-90F8-47E7239C6EA1")) ?? new SafeJournalItem();
        }

        public SafeMeetingItem CreateSafeMeetingItem()
        {
            return (SafeMeetingItem)Create(new Guid("FA2CBAFB-F7B1-4F41-9B7A-73329A6C1CB7")) ?? new SafeMeetingItem();
        }

        public SafePostItem CreateSafePostItem()
        {
            return (SafePostItem)Create(new Guid("11E2BC0C-5D4F-4E0C-B438-501FFE05A382")) ?? new SafePostItem();
        }

        public SafeReportItem CreateSafeReportItem()
        {
            return (SafeReportItem)Create(new Guid("D46BA7B2-899F-4F60-85C7-4DF5713F6F18")) ?? new SafeReportItem();
        }

        public MAPIFolder CreateMAPIFolder()
        {
            return (MAPIFolder)Create(new Guid("03C4C5F4-1893-444C-B8D8-002F0034DA92")) ?? new MAPIFolder();
        }

        public SafeCurrentUser CreateSafeCurrentUser()
        {
            return (SafeCurrentUser)Create(new Guid("7ED1E9B1-CB57-4FA0-84E8-FAE653FE8E6B")) ?? new SafeCurrentUser();
        }

        public SafeDistList CreateSafeDistList()
        {
            return (SafeDistList)Create(new Guid("7C4A630A-DE98-4E3E-8093-E8F5E159BB72")) ?? new SafeDistList();
        }

        public AddressLists CreateAddressLists()
        {
            return (AddressLists)Create(new Guid("37587889-FC28-4507-B6D3-8557305F7511")) ?? new AddressLists();
        }

        public MAPITable CreateMAPITable()
        {
            return (MAPITable)Create(new Guid("A6931B16-90FA-4D69-A49F-3ABFA2C04060")) ?? new MAPITable();
        }

        public MAPIUtils CreateMAPIUtils()
        {
            return (MAPIUtils)Create(new Guid("4A5E947E-C407-4DCC-A0B5-5658E457153B")) ?? new MAPIUtils();
        }

        public SafeInspector CreateSafeInspector()
        {
            return (SafeInspector)Create(new Guid("ED323630-B4FD-4628-BC6A-D4CC44AE3F00")) ?? new SafeInspector();
        }

        public SafeExplorer CreateSafeExplorer()
        {
            return (SafeExplorer)Create(new Guid("C3B05695-AE2C-4FD5-A191-2E4C782C03E0")) ?? new SafeExplorer();
        }

        public SafeApplication CreateSafeApplication()
        {
            return (SafeApplication)Create(new Guid("9DCB6F1D-9AB2-4002-A469-89A940E28A75")) ?? new SafeApplication();
        }

        #endregion
    }
}
