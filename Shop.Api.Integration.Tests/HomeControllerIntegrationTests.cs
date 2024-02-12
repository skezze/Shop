using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Api;
using Shop.Data;
using Shop.Domain.Models;

namespace Shop.Api.Integration.Tests;

[TestClass]
public class HomeControllerIntegrationTests
{
    [TestMethod]
    public async Task GetHello_SendingRequest_ShouldReturnNotAuthorized()
    {
        // Arrange

            WebApplicationFactory<Program> webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(builer =>
            {
                builer.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    services.Remove(dbContextDescriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("shop_db");
                    });
                });
            });

            
            HttpClient httpClient = webHost.CreateClient();

            // Act

            HttpResponseMessage response = await httpClient.GetAsync("api/home/get-hello");

            // Assert

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
}