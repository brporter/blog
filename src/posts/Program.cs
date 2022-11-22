using System.CommandLine;
using posts.Commands;
using posts.HostedServices;
using posts.Models;
using posts.Services;

namespace posts;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var databaseInitOption = new Option<bool>(
            name: "--initdb",
            description: "Initialize the database structure at the database server specified by the configuration connection string."
        );

        var rootCommand = new RootCommand();
        rootCommand.AddOption(databaseInitOption);

        rootCommand.SetHandler(async (initDb) =>
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton(new CommandLineParameters(initDb));
            builder.Services.AddServices();
            builder.Services.AddHostedServices();
            builder.Services.AddCommands();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }, databaseInitOption);

        return await rootCommand.InvokeAsync(args);
    }
}