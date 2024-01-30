using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Shop.Domain;
using Shop.Domain.ApiAuthConfiguration;
using Shop.Domain.FrontModels;
using Shop.Domain.Models;

namespace Shop.Api.Controllers;

using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserManager<User> _userManager;

    public AccountController(IHttpClientFactory httpClientFactory,UserManager<User> userManager)
    {
        _httpClientFactory = httpClientFactory;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] RegistrationModel model)
    {
        using (var client = _httpClientFactory.CreateClient())
        {
           
              var passwordTokenRequest = new PasswordTokenRequest
            {
                Address = $"{ApiAuthConfiguration.Authority}/connect/token",
                ClientId = ApiAuthConfiguration.ClientId,
                ClientSecret = ApiAuthConfiguration.ClientSecret,
                Scope = "Shop.Api",
                GrantType = GrantType.ResourceOwnerPassword,
                
                UserName = model.UserName,
                Password = model.Password
            };
            var tokenResponse = await client.RequestPasswordTokenAsync(passwordTokenRequest);
            if (tokenResponse.IsError)
            {
                // Обработка ошибок при регистрации
                
                return BadRequest($"Failed to register user.{tokenResponse.Error}");
            }

            // Далее, если успешно, вы можете вернуть токен или выполнить другие действия
            return Ok(new { access_token = tokenResponse.AccessToken });
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationModel model)
    {
        var result = await _userManager.CreateAsync(new User() {
            UserName = model.UserName,
        }, model.Password);
        return Ok(new {result = result.Succeeded});
    }
}