using Microsoft.AspNetCore.Identity;

namespace Shop.Application.Services;

public class RoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<bool> AddRole(string roleName)
    {
      
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        return false;
    }
}