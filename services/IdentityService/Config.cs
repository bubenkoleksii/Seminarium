﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;

using IdentityModel;

namespace IdentityService;

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
            new("SeminariumApp", "Seminarium app full access."),
        };

    public static IEnumerable<ApiResource> ApiResources = new List<ApiResource>
    {
        new("SeminariumApp", "Seminarium app", new []
        {
            JwtClaimTypes.Name,
            JwtClaimTypes.Role
        })
        {
            Scopes = { "SeminariumApp" }
        }
    };


    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new()
            {
                ClientId = "apiDog",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = false,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "SeminariumApp"
                }
            }
        };
}
