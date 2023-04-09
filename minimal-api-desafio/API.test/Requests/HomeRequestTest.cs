using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using MinimalApiDesafio;

namespace API.test.Requests;

[TestClass]
public class HomeResquestTest
{
    public const string PORT = "5001";
    public const string HOST = "http://localhost"; 
    private static TestContext _testContext = default!;
    private static WebApplicationFactory<Startup> _http = default!;

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        _testContext = testContext;
        _http = new WebApplicationFactory<Startup>();
    }

    [ClassCleanup]
    public static void ClassCleanup() 
    {
        _http.Dispose();
    }

    [TestMethod]
    public async Task TestaSeAHomeDaAPIExiste()
    {

        _http = _http.WithWebHostBuilder(builder =>{
            
            builder.UseSetting("https_port", PORT).UseEnvironment("Testing");
        });

        var client = _http.CreateClient();
        var response = await client.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}