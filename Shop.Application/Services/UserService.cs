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

    public async Task<bool> Login(User user)
    {
        var id = await _userManager.GetUserIdAsync(user);
        if (id==string.Empty)
        {
            return false;
        }
        await _signInManager.SignInAsync(user, isPersistent: true);
        return true;
    }

    public async Task<User?> Register(User user)
    {
        var id = await _userManager.GetUserIdAsync(user);
        if (id!=string.Empty)
        {
            return null;
        }
        await _signInManager.SignInAsync(user, isPersistent: true);
        return user;
    }
    public async Task<bool> AddUserToRole(User user,string[] roleNames)
    {
        var result = await _userManager.AddToRolesAsync(user, roleNames);
        if (result.Succeeded)
        {
            return true;
        }
        return false;
    }
}