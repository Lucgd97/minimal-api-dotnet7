using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API.test.Requests;

[TestClass]
public class HomeResquestTest
{
    private static TestContext _testContext;
    private static WebApplicationFactory<Startup> _factory;

    [TestMethod]
    public async Task TestaSeAHomeDaAPIExiste()
    {
        HttpClient cli = new HttpClient();
        var request = await cli.GetAsync("http://localhost:5287");

        Assert.AreEqual(HttpStatusCode.OK, request.StatusCode);
    }
}