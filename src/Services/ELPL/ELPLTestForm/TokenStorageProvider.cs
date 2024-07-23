using System.Collections.Generic;
using ELPLServicesLibrary;

namespace ELPLTestForm
{
    class TokenStorageProvider : ITokenStorageProvider
    {
        private static readonly Dictionary<int, TokenDetails> _tokenStorage = new Dictionary<int, TokenDetails>();

        public TokenDetails LoadToken(int? msUserId)
        {
            TokenDetails tokenDetails;
            _tokenStorage.TryGetValue(msUserId.GetValueOrDefault(), out tokenDetails);
            return tokenDetails;
        }

        public void StoreToken(int? msUserId, TokenDetails tokenDetails)
        {
            _tokenStorage[msUserId.GetValueOrDefault()] = tokenDetails;
        }
    }
}
