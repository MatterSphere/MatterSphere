using System;
using System.Collections.Generic;
using System.Linq;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList
{
    public class FavoritesProvider
    {
        private const string _fileType = "CLINETFILEFT";
        private const string _clientType = "CLINETFT";
        private const string _precedentType = "PRECFAV";
        private const string _favoriteDescription = "MYFAV";
        private const string _precFavoriteDescription = "PRECFAVTAB";

        public EventHandler FilesUpdated;

        #region Files

        private List<FileData> _favoritesFiles;
        public List<FileData> FavoritesFiles
        {
            get
            {
                if (_favoritesFiles == null)
                {
                    UpdateFavoritesFiles();
                }

                return _favoritesFiles;
            }
        }

        public bool RemoveFileFavorite(string clientNo, string fileNo, BaseContainerPage sender)
        {
            var favId = FavoritesFiles.First(fav => fav.ClientNo == clientNo && fav.FileNo == fileNo).FavoriteId;
            var favorites = new Favourites(_fileType, _favoriteDescription);
            var result = favorites.RemoveFavourite(favId.ToString());
            UpdateFavoritesFiles();
            FilesUpdated?.Invoke(this, EventArgs.Empty);

            return result;
        }

        public bool AddFileFavorite(string clientNo, string fileNo, string fileDescription, BaseContainerPage sender)
        {
            var favorites = new Favourites(_fileType);
            var result = favorites.AddFavourite(_favoriteDescription, "2", new[] { fileDescription, clientNo, fileNo });
            UpdateFavoritesFiles();
            FilesUpdated?.Invoke(this, EventArgs.Empty);

            return result;
        }

        public void UpdateFavoritesFiles()
        {
            var favorites = new Favourites(_fileType, _favoriteDescription);

            var files = new List<FileData>();
            for (int i = 0; i < favorites.Count; i++)
            {
                files.Add(new FileData(favorites.FavoriteId(i), favorites.Param2(i), favorites.Param3(i)));
            }

            _favoritesFiles = files;
        }

        public bool HasFile(string clientNo, string fileNo)
        {
            return FavoritesFiles.Any(it => it.FileNo == fileNo && it.ClientNo == clientNo);
        }

        #endregion

        #region Clients

        private List<ClientData> _favoritesClients;
        public List<ClientData> FavoritesClients
        {
            get
            {
                if (_favoritesClients == null)
                {
                    UpdateFavoritesClients();
                }

                return _favoritesClients;
            }
        }

        public bool RemoveClientFavorite(string clientNo)
        {
            var favId = FavoritesClients.First(fav => fav.ClientNo == clientNo).FavoriteId;
            var favorites = new Favourites(_clientType, _favoriteDescription);
            var result = favorites.RemoveFavourite(favId.ToString());
            UpdateFavoritesClients();

            return result;
        }

        public bool AddClientFavorite(string clientNo, string clientDescription)
        {
            var favorites = new Favourites(_clientType);
            var result = favorites.AddFavourite(_favoriteDescription, "2", new[] { clientDescription, clientNo });
            UpdateFavoritesClients();

            return result;
        }

        public void UpdateFavoritesClients()
        {
            var favorites = new Favourites(_clientType, _favoriteDescription);

            var clients = new List<ClientData>();
            for (int i = 0; i < favorites.Count; i++)
            {
                clients.Add(new ClientData(favorites.FavoriteId(i), favorites.Param2(i)));
            }

            _favoritesClients = clients;
        }

        public bool HasClient(string clientNo)
        {
            return FavoritesClients.Any(it => it.ClientNo == clientNo);
        }

        #endregion

        #region Precedents

        private List<PrecedentData> _favoritesPrecedents;
        public List<PrecedentData> FavoritesPrecedents
        {
            get
            {
                if (_favoritesPrecedents == null)
                {
                    UpdateFavoritesPrecedents();
                }

                return _favoritesPrecedents;
            }
        }

        public bool RemovePrecedentFavorite(long id)
        {
            var favId = FavoritesPrecedents.First(fav => fav.PrecedentId == id).FavoriteId;
            var favorites = new Favourites(_precedentType, _precFavoriteDescription);
            var result = favorites.RemoveFavourite(favId.ToString());
            UpdateFavoritesPrecedents();

            return result;
        }

        public bool AddPrecedentFavorite(long id)
        {
            var favorites = new Favourites(_precedentType);
            var result = favorites.AddFavourite(_precFavoriteDescription, string.Empty, new[] { id.ToString() });
            UpdateFavoritesPrecedents();

            return result;
        }

        public void UpdateFavoritesPrecedents()
        {
            var favorites = new Favourites(_precedentType, _precFavoriteDescription);

            var precedents = new List<PrecedentData>();
            for (int i = 0; i < favorites.Count; i++)
            {
                precedents.Add(new PrecedentData(favorites.FavoriteId(i), Convert.ToInt64(favorites.Param1(i))));
            }

            _favoritesPrecedents = precedents;
        }

        public bool HasPrecedent(long precId)
        {
            return FavoritesPrecedents.Any(it => it.PrecedentId == precId);
        }

        #endregion

        #region Classes

        public class FileData
        {
            public FileData(int favoriteId, string client, string file)
            {
                FavoriteId = favoriteId;
                ClientNo = client;
                FileNo = file;
            }

            public int FavoriteId { get; private set; }
            public string ClientNo { get; private set; }
            public string FileNo { get; private set; }
        }

        public class ClientData
        {
            public ClientData(int favoriteId, string client)
            {
                FavoriteId = favoriteId;
                ClientNo = client;
            }

            public int FavoriteId { get; private set; }
            public string ClientNo { get; private set; }
        }

        public class PrecedentData
        {
            public PrecedentData(int favoriteId, long precId)
            {
                FavoriteId = favoriteId;
                PrecedentId = precId;
            }

            public int FavoriteId { get; private set; }
            public long PrecedentId { get; private set; }
        }

        #endregion
    }
}
