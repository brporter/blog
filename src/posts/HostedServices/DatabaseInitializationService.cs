using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Dapper;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.Data.SqlClient;
using posts.Models;
using posts.Services;

namespace posts.HostedServices;

public class DatabaseInitializationService
    : IHostedService
{
    private readonly ILogger<DatabaseInitializationService> _logger;
    private readonly DataConnectionBroker _broker;
    private readonly CommandLineParameters _parameters;

    public DatabaseInitializationService(ILogger<DatabaseInitializationService> logger, DataConnectionBroker broker, CommandLineParameters parameters)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _broker = broker ?? throw new ArgumentNullException(nameof(broker));
        _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_parameters.InitializeDatabase)
        {
            _logger.LogInformation("Skipping database initialization.");
            return;
        }

        _logger.LogCritical("Database initialization starting...");

        using var connection = _broker.GetConnection();
        connection.Open();

        var assembly = typeof(DatabaseInitializationService).Assembly;
        var resources = assembly.GetManifestResourceNames();

        var sqlResources = resources.Where(r => r.EndsWith(".sql")).ToImmutableSortedSet();
        var cleanProcedures = sqlResources.Where(r => r.EndsWith("clean.sql")).ToImmutableSortedSet();
        var sqlTables = sqlResources.Where(r => r.Contains("Tables")).ToImmutableSortedSet();
        var sqlProcedures = sqlResources.Where(r => r.Contains("Procedures")).ToImmutableSortedSet();
        
        // Clean
        await ProcessSqlCommands(assembly, connection, cleanProcedures, cancellationToken);

        // Tables
        await ProcessSqlCommands(assembly, connection, sqlTables, cancellationToken);
        
        // Stored Procedures
        await ProcessSqlCommands(assembly, connection, sqlProcedures, cancellationToken);

        _logger.LogCritical("Database initialization complete.");
    }

    private async Task ProcessSqlCommands(Assembly assembly, IDbConnection connection, ImmutableSortedSet<string> sqlResources, CancellationToken cancellationToken)
    {
        foreach (var sqlResource in sqlResources)
        {
            var resourceStream = assembly.GetManifestResourceStream(sqlResource) ??
                                 throw new InvalidOperationException(
                                     "A named SQL resource was found but the resource stream could not be retrieved.");

            var streamReader = new StreamReader(resourceStream);
            var sqlCommand = await streamReader.ReadToEndAsync(cancellationToken);

            if (cancellationToken.IsCancellationRequested) return;

            try
            {
                await connection.ExecuteAsync(sqlCommand);
            }
            catch (SqlException se)
            {
                _logger.LogError(se, "Failed to execute initialization script '{0}'", sqlResource);
                throw;
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}