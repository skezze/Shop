using Microsoft.AspNetCore.Identity;
using Shop.Domain.Models;

namespace Shop.Application.Services;

public class UserService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserService(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<IdentityResult> Login(User user)
    {
        var id = await _userManager.GetUserIdAsync(user);
        if (id==string.Empty)
        {
            return IdentityResult.Failed();
        }
        await _signInManager.SignInAsync(user, isPersistent: true);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> Register(User user, string password)
    {
        var id = await _userManager.GetUserIdAsync(user);
        if (id!=string.Empty)
        {
            return IdentityResult.Failed();
        }
        await _userManager.CreateAsync(new User() {
            UserName = user.UserName,
        }, password);
        return IdentityResult.Success;
    }
    public async Task<IdentityResult> AddUserToRole(User user,string[] roleNames)
    {
        var result = await _userManager.AddToRolesAsync(user, roleNames);
        if (result.Succeeded)
        {
            return IdentityResult.Success;
        }
        return IdentityResult.Failed();
    }
}