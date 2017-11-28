using Nop.Services.Authentication.External;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.ExternalAuth.QQConnect.Core
{
    [Serializable]
    public class OAuthAuthenticationParameters : OpenAuthenticationParameters
    {
        private readonly string _providerSystemName;
        private IList<UserClaims> _claims;

        public OAuthAuthenticationParameters(string providerSystemName)
        {
            _providerSystemName = providerSystemName;
        }


        public virtual IList<UserClaims> UserClaims => _claims;

        public void AddClaim(UserClaims claim)
        {
            if (_claims == null)
                _claims = new List<UserClaims>();
            _claims.Add(claim);
        }

        public override string ProviderSystemName => _providerSystemName;
    }
}
