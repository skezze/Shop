namespace Shop.Api.Models.ViewModels;

public class UserView
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set;}
    public bool EmailConfirmed { get; set; } = true;
}