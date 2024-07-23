using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public interface IStorageItemVersion : IStorageItem
    {
        Guid Id{get;}
        Guid? ParentId { get;}
        IStorageItem BaseStorageItem { get;}
        int Version{get;}
        bool IsSubVersion { get;}
        string Label { get;set;}
        bool IsLatestVersion { get;}
        string Comments { get;set;}
        string Status { get;set;}

        string CreatedBy { get;}
        string LastUpdatedBy { get;}
        DateTime? Created { get;}
        DateTime? LastUpdated { get;}


    }
}
