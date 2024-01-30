// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;
using Shop.Domain;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("Shop.Api"),
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("Shop.Api", new []
                    { JwtClaimTypes.Name})
                {
                    Scopes =
                    {
                        "Shop.Api_Read",
                        "Shop.Api_Update",
                        "Shop.Api_Create",
                        "Shop.Api_Delete"
                    }
                },
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "Shop.Api.Client",
                    ClientName = "Shop.Api",
                    
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = true,
                    ClientSecrets = new []
                         {
                              new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())
                         },
                    AllowedScopes = { ApiScopesList.ShopApi},
                },

            };
    }
}