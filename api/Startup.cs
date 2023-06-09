using Microsoft.AspNetCore.Mvc;
using MinimalApiDesafio.ModelViews;
using MinimalApiDesafio.DTOs;
using MinimalApiDesafio.Models;
using Microsoft.OpenApi.Models;
using minimal_api_desafio.Database;
using Microsoft.EntityFrameworkCore;

namespace MinimalApiDesafio;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minimal API Desafio", Version = "v1" });
        });

        services.AddEndpointsApiExplorer();

        string? conexao = Environment.GetEnvironmentVariable("DATABASE_URL_MINIMAL_API");
        services.AddDbContext<DbContexto>(options =>
        {
            options.UseMySql(conexao, ServerVersion.AutoDetect(conexao));
        });

        //services.AddScoped<IStudentsService, StudentsService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API Desafio v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            MapRoutes(endpoints);
            MapRoutesClientes(endpoints);
        });
    }

    
    #region Rotas utilizando Minimal API

    public void MapRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => new {Mensagem = "Bem vindo a API"})
            .Produces<dynamic>(StatusCodes.Status200OK)
            .WithName("Home")
            .WithTags("Testes");

        app.MapGet("/recebe-parametro", (string? nome) => 
        {
            if(string.IsNullOrEmpty(nome))
            {
                return Results.BadRequest(new {
                    Mesagem = "Olha você não mandou uma informação importante, o nome é obrigatório"
                });
            }

            nome = $""" 
            Alterando parametro recebido {nome}
            """;

            var objetoDeRetono = new {
                ParametroPassado = nome,
                Mensagem = "Muito bem alunos passamos um parametro por querystring"
            };

            return Results.Created($"/recebe-parametro/{objetoDeRetono.ParametroPassado}", objetoDeRetono);
        })
        .Produces<dynamic>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("TesteRebeParametro")
        .WithTags("Testes");
    }


    public void MapRoutesClientes(IEndpointRouteBuilder app)
    {
        app.MapGet("/clientes", () => 
        {
            var clientes = new List<Cliente>();
            clientes.Add(new Cliente() 
            {
                Id = 1,
                Nome = "Janaina",
                Email = "jan@gmail.com",
                Telefone = "(11)11111-11111"
            });
            // var clientes = ClienteService.Todos();

            return Results.Ok(clientes);
        })
        .Produces<List<Cliente>>(StatusCodes.Status200OK)
        .WithName("GetClientes")
        .WithTags("Clientes");

        app.MapPost("/clientes", ([FromBody] ClienteDTO clienteDTO) => 
        {
            var cliente = new Cliente
            {
                Nome = clienteDTO.Nome,
                Telefone = clienteDTO.Telefone,
                Email = clienteDTO.Email,
            };
            // ClienteService.Salvar(cliente);

            return Results.Created($"/cliente/{cliente.Id}", cliente);
        })
        .Produces<Cliente>(StatusCodes.Status201Created)
        .Produces<Error>(StatusCodes.Status400BadRequest)
        .WithName("PostClientes")
        .WithTags("Clientes");

        app.MapPut("/clientes/{id}", ([FromRoute] int id, [FromBody] ClienteDTO clienteDTO) => 
        {
            if(string.IsNullOrEmpty(clienteDTO.Nome))
            {
                return Results.BadRequest(new Error 
                { 
                    Codigo = 123432, 
                    Mensagem = "O Nome é obrigatório" 
                });
            }

            /*

            var cliente = ClienteService.BuscaPorId(id);
            if(cliente == null)
                return Results.NotFound(new Error { Codigo = 123432, Mensagem = "Você passou um cliente inexistente" });

            cliente.Nome = clienteDTO.Nome;
            cliente.Telefone = clienteDTO.Telefone;
            cliente.Email = clienteDTO.Email;

            ClienteService.Update(cliente);
            */

            var cliente = new Cliente();

            return Results.Ok(cliente);
        })
        .Produces<Cliente>(StatusCodes.Status200OK)
        .Produces<Error>(StatusCodes.Status404NotFound)
        .Produces<Error>(StatusCodes.Status400BadRequest)
        .WithName("PutClientes")
        .WithTags("Clientes");

        app.MapPatch("/clientes/{id}", ([FromRoute] int id, [FromBody] ClienteNomeDTO clienteNomeDTO) => 
        {
            Console.WriteLine($"===========[{clienteNomeDTO.Nome}]==========");
            if(string.IsNullOrEmpty(clienteNomeDTO.Nome))
            {
                return Results.BadRequest(new Error 
                { 
                    Codigo = 123, 
                    Mensagem = "O Nome é obrigatório" 
                });
            }

            /*
            ClienteService.Update(cliente);
            */

            var cliente = new Cliente();

            return Results.Ok(cliente);
        })
        .Produces<Cliente>(StatusCodes.Status200OK)
        .Produces<Error>(StatusCodes.Status404NotFound)
        .Produces<Error>(StatusCodes.Status400BadRequest)
        .WithName("PatchClientes")
        .WithTags("Clientes");

        app.MapDelete("/clientes/{id}", ([FromRoute] int id) => 
        {
            if(id == 4)
            {
                return Results.NotFound(new Error 
                { 
                    Codigo = 12, 
                    Mensagem = "Cliente não encontrado" 
                });
            }

            // FAZER CÓDIGO PARA EXCLUIR DO BANCO

            return Results.NoContent();
        })
        .Produces<Cliente>(StatusCodes.Status204NoContent)
        .Produces<Error>(StatusCodes.Status404NotFound)
        .WithName("DeleteClientes")
        .WithTags("Clientes");


        app.MapGet("/clientes/{id}", ([FromRoute] int id) => 
        {
            if(id == 4)
            {
                return Results.NotFound(new Error 
                { 
                    Codigo = 12, 
                    Mensagem = "Cliente não encontrado" 
                });
            }

            return Results.Ok(new Cliente(){
                Id = 1,
                Nome = "Danilo",
                Telefone = "111111111",
                Email = "Danilo@teste.com",
            });
        })
        .Produces<Cliente>(StatusCodes.Status204NoContent)
        .Produces<Error>(StatusCodes.Status404NotFound)
        .WithName("GetClientesPorId")
        .WithTags("Clientes");

    }

    #endregion
}