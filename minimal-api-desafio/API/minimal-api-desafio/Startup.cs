using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using MinimalApiDesafio.ModelViews;
using MinimalApiDesafio.Models;
using MinimalApiDesafio.DTOs;

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
            c.SwaggerDoc("v1", new OpenApiInfo{ Title = "Minimal Api Desafio", Version = "v1"});
        });

        builder.Services.AddEndpointsApiExplorer();
        //services.AddScoped<IStudentsService>, StudentesService();
    }
}