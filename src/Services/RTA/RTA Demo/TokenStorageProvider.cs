using System.Collections.Generic;
using RTAServicesLibrary;

namespace RTA_Demo
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
