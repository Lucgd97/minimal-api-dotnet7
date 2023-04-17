using System.Net;
using API.Test.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using MinimalApiDesafio;

namespace API.test.Requests;

[TestClass]
public class ClientesResquestTest
{
    
    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [ClassCleanup]
    public static void ClassCleanup() 
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task RotaClientes()
    {
        var response = await Setup.client.GetAsync("/clientes");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("""[]""", result);
    }

    
}