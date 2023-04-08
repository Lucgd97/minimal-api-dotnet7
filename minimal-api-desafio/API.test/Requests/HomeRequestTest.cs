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
    private static TestContext _testContext;
    private static WebApplicationFactory<Startup> _http;

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        _testContext = testContext;
        _http = new WebApplicationFactory<Startup>();
    }

    [TestMethod]
    public async Task TestaSeAHomeDaAPIExiste()
    {

        _http = _http.WithWebHostBuilder(builder =>{
            
            builder.UseSetting("https_port", PORT).UseEnvironment("Testing");
        });

        var cli = _http.CreateClient();
        var request = await cli.GetAsync($"{HOST}:{PORT}");

        Assert.AreEqual(HttpStatusCode.OK, request.StatusCode);
    }
}