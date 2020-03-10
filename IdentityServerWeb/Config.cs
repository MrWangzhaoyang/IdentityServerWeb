using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerWeb
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }
        public static IEnumerable<ApiResource> GetApiResource()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "My Api")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) }
                }
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
                {
                    new Client
                    {
                        ClientId = "mvc",
                        ClientName = "mvc Client",
                        ClientUri ="https://localhost:5001",
                        LogoUri ="https://www.easyicon.net/api/resizeApi.php?id=1241443&size=128",
                        ClientSecrets = { new Secret("secret".Sha256()) },
                        AllowedGrantTypes = GrantTypes.Implicit,
                        RequireConsent = true,
                        RedirectUris = { "https://localhost:5001/signin-oidc"},
                        PostLogoutRedirectUris ={ "https://localhost:5001/signout-callback-oidc"},
                        AllowedScopes = { 
                            //"api"
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                        },
                    }
                };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "10000",
                        Username = "zhaoyang",
                        Password = "123456"
                    },
                };
        }
    }
}
