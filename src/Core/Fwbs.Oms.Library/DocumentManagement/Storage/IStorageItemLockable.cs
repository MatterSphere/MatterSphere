using System;

namespace FWBS.OMS.DocumentManagement.Storage
{

    public interface IStorageItemLockable
    {
        DateTime? CheckedOutTime { get;}
        int CheckOutDuration { get;}
        
        bool IsCheckedOut { get;}
        bool IsCheckedOutByCurrentUser { get;}
        bool IsCheckedOutByAnother { get;}
        bool CanCheckOut { get;}
        bool CanCheckIn { get;}
        bool CanUndo { get;}

        User CheckedOutBy { get;}

        string CheckedOutMachine { get;}
        string CheckedOutLocation { get;}

        void CheckOut(System.IO.FileInfo localFile);
        void CheckIn();
        void UndoCheckOut();
        void UpdateCheckedOutLocation(System.IO.FileInfo localFile);
    }
}
