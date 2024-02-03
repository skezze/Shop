using IdentityServer4.Models;

namespace Shop.Domain.ApiAuthConfiguration;

public static class ApiAuthConfiguration
{
    public static string Authority = "https://localhost:5001";
    public static string ClientId = "Shop.Api.Client";
    public static string Granttype = GrantType.ResourceOwnerPassword;
    public static string ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A";
}