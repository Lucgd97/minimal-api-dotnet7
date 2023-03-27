using Microsoft.AspNetCore.Mvc;
using MinimalApiDesafio.ModelViews;
using MinimalApiDesafio.Models;
using MinimalApiDesafio.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MapRoutes(app);

MapRoutesClientes(app);

app.Run();

#region Rotas utilizando Mininal API

void MapRoutes(WebApplication app)
{
    app.MapGet("/", () => new {Mensagem = "Bem vindo a Api"})// rota minima para api
    .Produces<dynamic>(StatusCodes.Status200OK)
    .WithName("Home")
    .WithTags("Testes"); 


    app.MapGet("/recebe-parametro", (HttpRequest request, HttpResponse response, string? nome) =>
    {

        if(string.IsNullOrEmpty(nome))
        {
            return Results.BadRequest(new{
                Mensagem = "Olha você não mandou uma informação importante, o nome é obrigatório"
            });
            
        }

        nome = $""" 
        Alterando parametro recebido {nome}
        """;

        var objetoDeRetorno = new {
            ParametroPassado = nome,
            Mensagem = "Muito bem alunos passamos um parametro por querystring"
        };

        return Results.Created($"/recebe-parametro/{objetoDeRetorno.ParametroPassado}", objetoDeRetorno);
    })
    .Produces<dynamic>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("TesteRecebeParametro")
    .WithTags("Testes");
}

void MapRoutesClientes(WebApplication app) 
{
    // rota Get - retornar informação
    app.MapGet("/clientes", () => 
    {
        var clientes = new List<Cliente>();
        // var clientes = ClienteService.Todos();
        return Results.Ok(clientes);
    })
    .Produces<Cliente>(StatusCodes.Status200OK)
    .WithName("GetClientes")
    .WithTags("Clientes");

    // rota post
    app.MapPost("/clientes", ([FromBody] ClienteDTO clienteDTO) => 
    {
        var cliente = new Cliente
        {
            Nome = clienteDTO.Nome,
            Telefone = clienteDTO.Telefone,
            Email = clienteDTO.Email,
        };
        // cliente.salvar(cliente)

        return Results.Created($"/cliente/{cliente.Id}", cliente);
    })
    .Produces<dynamic>(StatusCodes.Status201Created)
    .Produces<Error>(StatusCodes.Status400BadRequest)
    .WithName("PostClientes")
    .WithTags("Clientes");

    // rota put - alterar
    app.MapPut("/clientes/{id}", ([FromRoute] int id, [FromBody] ClienteDTO clienteDTO) => 
    {

        if(!string.IsNullOrEmpty(clienteDTO.Nome))
        {
            return Results.BadRequest(new Error
            {
                Codigo = 123432,
                Mensagem = "Nome é obrigatório"
            });
        }

        /*var cliente = ClienteService.BuscarPorId(id);
        if(cliente == null)
            return Results.NotFound(new Error {Codigo = 123432, Mensagem = "Você passou um cliente inexistente"});

        cliente.Nome = clienteDTO.Nome;
        cliente.Telefone = clienteDTO.Telefone;
        cliente.Email = clienteDTO.Email;
        ClienteService.Update(cliente);*/    

        var cliente = new Cliente();
       
        return Results.Ok(cliente);
    })
    .Produces<Cliente>(StatusCodes.Status200OK)
    .Produces<Error>(StatusCodes.Status404NotFound)
    .Produces<Error>(StatusCodes.Status400BadRequest)
    .WithName("PutClientes")
    .WithTags("Clientes");
}

#endregion


