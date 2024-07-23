using System;
namespace FWBS.OMS.Dashboard
{
    public class FavoriteRow
    {
        private string _description;

        public FavoriteRow(int id, string description, FavoriteItemType type, string combo, DateTime modified, string extension = null)
        {
            Id = id;
            _description = description;
            ItemType = type;
            Combo = combo;
            Modified = modified;
            Extension = extension;
        }

        public int Id { get; private set; }
        public string ClientNo { get; set; }
        public string FileNo { get; set; }
        
        public FavoriteItemType ItemType { get; private set; }
        public DateTime Modified { get; private set; }
        public string Extension { get; private set; }
        public string Combo { get; private set; }

        public long? ClientId { get; set; }
        public long? FileId { get; set; }
        public long? PrecedentId { get; set; }

        public string Icon
        {
            get
            {
                switch (ItemType)
                {
                    case FavoriteItemType.Client:
                        return "CLIENT";
                    case FavoriteItemType.File:
                        return "FILE";
                    default:
                        return Extension;
                }
            }
        }

        public string Description
        {
            get
            {
                switch (ItemType)
                {
                    case FavoriteItemType.Client:
                        return $"{ClientNo} : {_description}";
                    case FavoriteItemType.File:
                        return $"{ClientNo}/{FileNo} : {_description}";
                    case FavoriteItemType.Precedent:
                        return $"{_description}.{Extension}";
                }

                return null;
            }
        }
        
        public enum FavoriteItemType
        {
            Client,
            File,
            Precedent
        }
    }
}
