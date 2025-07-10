using System.Transactions;
using InternshipTask.Infrastructure.Repositories.Interfaces;
using InternshipTask.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Npgsql;

namespace InternshipTask.Infrastructure.Common;

public abstract class PgRepository : IPgRepository
{
    private readonly DbOptions _options;
    
    protected const int DefaultTimeoutInSeconds = 5;
    
    protected PgRepository(IOptions<DbOptions> options)
    {
        _options = options.Value;
    }

    protected async Task<NpgsqlConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (Transaction.Current is not null &&
            Transaction.Current.TransactionInformation.Status is TransactionStatus.Aborted)
        {
            throw new TransactionAbortedException("Transaction was aborted (probably by user cancellation request)");
        }
        
        var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        
        await connection.ReloadTypesAsync(cancellationToken);
        
        return connection;
    }
    
}