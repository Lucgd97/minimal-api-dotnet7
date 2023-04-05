using Microsoft.AspNetCore.Mvc;
using MinimalApiDesafio.ModelViews;
using MinimalApiDesafio.Models;
using MinimalApiDesafio.DTOs;
using Microsoft.OpenApi.Models;

namespace MinimalApiDesafio;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration{ get; }

    public void ConfigurationServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minimal Api Desafio", Version = "v1"});
        });

        builder.Services.AddEndpointsApiExplorer();
        //services.AddScoped<IStudentsService>, StudentesService();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal Api Desafio v1"));
        }

        app.UseHttpsRedirection();
        
        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        MapRoutes(app);
        MapRoutesClientes(app);
    }

    
    #region Rotas utilizando Mininal API

    public void MapRoutes(WebApplication app)
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

    public void MapRoutesClientes(WebApplication app) 
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
}