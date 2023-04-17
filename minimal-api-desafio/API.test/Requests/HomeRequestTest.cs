using System.Net;
using API.Test.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using MinimalApiDesafio;

namespace API.test.Requests;

[TestClass]
public class HomeResquestTest
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
    public async Task TestaSeAHomeDaAPIExiste()
    {
        var response = await Setup.client.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("""{"mensagem":"Bem vindo a API"}""", result);
    }

    [TestMethod]
    public async Task TestandoCaminhoFelizParaReceberParametro()
    {
        var response = await Setup.client.GetAsync("/recebe-parametro?nome=Leandro");

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("""{"parametroPassado":"Alterando parametro recevido Leandro","mensagem":"Muito bem alunos passamos um parametro por querystring"}""", result);
    }

    [TestMethod]
    public async Task TestandoParaReceberParametroSemOParametro()
    {
        var response = await Setup.client.GetAsync("/recebe-parametro");
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);    

        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("""{"mensagem":"Olha você não mandou uma informação importante, o nome é obrigátorio"}""", result);   
    }
}