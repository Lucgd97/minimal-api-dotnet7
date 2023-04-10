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
    private static HttpClient _client = default!;

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        _testContext = testContext;
        _http = new WebApplicationFactory<Startup>();

        _http = _http.WithWebHostBuilder(builder =>{
            
            builder.UseSetting("https_port", PORT).UseEnvironment("Testing");
        });

        _client = _http.CreateClient();
    }

    [ClassCleanup]
    public static void ClassCleanup() 
    {
        _http.Dispose();
    }

    [TestMethod]
    public async Task TestaSeAHomeDaAPIExiste()
    {
        var response = await _client.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("""{"mensagem":"Bem vindo a API"}""", result);
    }

    [TestMethod]
    public async Task TestandoCaminhoFelizParaReceberParametro()
    {
        var response = await _client.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("""{"mensagem":"Bem vindo a API"}""", result);
    }
}