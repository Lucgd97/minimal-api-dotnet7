using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.Test.Helpers;
//using API.Test.Mock;
using MinimalApiDesafio.DTOs;
using MinimalApiDesafio.Models;
//using MinimalApiDesafio.Servicos

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
    public async Task GetClientes()
    {   
        await SetHeaderToken();
        var response = await Setup.client.GetAsync("/clientes");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        var clientes = JsonSerializer.Deserialize<List<Cliente>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(clientes);
        Assert.IsTrue(clientes.Count > 0);
        Assert.IsNotNull(clientes[0].Id);
        Assert.IsNotNull(clientes[0].Nome);
        Assert.IsNotNull(clientes[0].Email);
        Assert.IsNotNull(clientes[0].Telefone);
        Assert.IsNotNull(clientes[0].DataCriacao);
    }
    

    [TestMethod]
    public async Task PostClientes()
    {
        await SetHeaderToken();
        var cliente = new ClienteDTO()
        {
            Nome = "Janaina",
            Email = "jan@gmail.com",
            Telefone = "(11)11111-11111"
        };

        var content = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");
        var response = await Setup.client.PostAsync("/clientes", content);

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        var clienteResponse = JsonSerializer.Deserialize<Cliente>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(clienteResponse);
        Assert.AreEqual(1, clienteResponse.Id);
    }

    [TestMethod]
    public async Task PutClientes()
    {
        await SetHeaderToken();
        var cliente = new ClienteDTO()
        {
            Nome = "Janaina",
            Email = "jan@gmail.com",
            Telefone = "(11)11111-11111"
        };

        var content = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");
        var response = await Setup.client.PutAsync($"/clientes/{1}", content);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        var clienteResponse = JsonSerializer.Deserialize<Cliente>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(clienteResponse);
        Assert.AreEqual(1, clienteResponse.Id);
        Assert.AreEqual("Janaina", clienteResponse.Nome);
    }

    [TestMethod]
    public async Task PutClientesSemNome()
    {
        await SetHeaderToken();
        var cliente = new ClienteDTO()
        {
            Email = "jan@gmail.com",
            Telefone = "(11)11111-11111"
        };

        var content = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");
        var response = await Setup.client.PutAsync($"/clientes/{1}", content);

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("""{"codigo":123432,"mensagem":"O Nome é obrigatório"}""", result);
    }

    [TestMethod]
    public async Task DeleteClientes()
    {
        var response = await Setup.client.DeleteAsync($"/clientes/{1}");
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }

    [TestMethod]
    public async Task DeleteClientesIdNaoExistente()
    {
        var response = await Setup.client.DeleteAsync($"/clientes/{4}");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task PatchClientes()
    {
        await SetHeaderToken();
        var cliente = new ClienteNomeDTO()
        {
            Nome = "Jaziel"
        };

        var content = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");
        var response = await Setup.client.PatchAsync($"/clientes/{1}", content);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("""{"codigo":123,"mensagem":"O Nome é obrigatório"}""", result);
    }

    
}