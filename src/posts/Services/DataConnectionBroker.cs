using System.Data;
using System.Data.Common;

namespace posts.Services;

public class DataConnectionBroker
{
    private readonly DbProviderFactory _factory;
    readonly string _connectionString;

    public DataConnectionBroker(DbProviderFactory factory, IConfiguration configuration)
    {
        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));

        var connectionString = configuration.GetConnectionString("Posts");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(configuration), "No connection string was specified.");
        }

        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        var connection = _factory.CreateConnection();

        if (connection == null)
        {
            throw new InvalidOperationException("Created connection object was null.");
        }

        connection.ConnectionString = _connectionString;
        
        return connection;
    }
};
