using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shop.Data;
using Shop.Domain.ApiAuthConfiguration;
using Shop.Domain.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = ApiAuthConfiguration.Authority;
        options.ClientId = ApiAuthConfiguration.ClientId;
        options.ClientSecret = ApiAuthConfiguration.ClientSecret;
        options.ResponseType = ApiAuthConfiguration.ResponseType;
        
        options.SaveTokens = ApiAuthConfiguration.SaveTokens;

        options.Scope.Add("Shop.Api");
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Shop.Api", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "Shop.Api");
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var origins = "_origins";
builder.Services.AddCors(options=>
{
   options.AddPolicy(origins, policy =>
   {
       policy.AllowAnyHeader();
       policy.AllowAnyOrigin();
       policy.AllowAnyMethod();
       policy.AllowCredentials();
   });
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(origins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
