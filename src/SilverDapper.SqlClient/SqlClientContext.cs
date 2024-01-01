using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace SilverDapper.SqlClient;

internal class SqlClientContext : IDapperContext
{
    private SqlConnection connection = null!;
    private bool disposed = false;

    private readonly SqlClientOptions options;

    public SqlClientContext(SqlClientOptions options)
    {
        connection = new SqlConnection(options.ConnectionString);
        this.options = options;
    }

    private IDbConnection Connection
    {
        get
        {
            if (connection.State is ConnectionState.Closed)
            {
                connection.Open();
            }

            return connection;
        }
    }

    public async Task<IDataReader> ExecuteReaderAsync(string sql, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null)
    {
        ThrowIfDisposed();
        return await Connection.ExecuteReaderAsync(sql, param, transaction, options.CommandTimeout, commandType);
    }

    public async Task<IDbConnection> GetConnectionAsync()
    {
        ThrowIfDisposed();

        await connection.OpenAsync();
        return connection;
    }

    public async Task<IEnumerable<T>?> GetDataAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null)
        where T : class
    {
        ThrowIfDisposed();
        return await Connection.QueryAsync<T>(sql, param, transaction, options.CommandTimeout, commandType);
    }

    public async Task<IEnumerable<TReturn>?> GetDataAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TReturn : class
    {
        ThrowIfDisposed();
        return await Connection.QueryAsync(sql, map, param, transaction, options.Buffered, splitOn, options.CommandTimeout, commandType);
    }

    public async Task<IEnumerable<TReturn>?> GetDataAsync<TFirst, TSecond, TThrid, TReturn>(string sql, Func<TFirst, TSecond, TThrid, TReturn> map, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThrid : class
        where TReturn : class
    {
        ThrowIfDisposed();
        return await Connection.QueryAsync(sql, map, param, transaction, options.Buffered, splitOn, options.CommandTimeout, commandType);
    }

    public async Task<IEnumerable<TReturn>?> GetDataAsync<TFirst, TSecond, TThrid, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThrid, TFourth, TReturn> map, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThrid : class
        where TFourth : class
        where TReturn : class
    {
        ThrowIfDisposed();
        return await Connection.QueryAsync(sql, map, param, transaction, options.Buffered, splitOn, options.CommandTimeout, commandType);
    }

    public async Task<T?> GetObjectAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null)
        where T : class
    {
        ThrowIfDisposed();
        return await Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, options.CommandTimeout, commandType);
    }

    public async Task<TReturn?> GetObjectAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TReturn : class
    {
        ThrowIfDisposed();

        var result = await Connection.QueryAsync(sql, map, param, transaction, options.Buffered, splitOn, options.CommandTimeout, commandType);
        return result.FirstOrDefault();
    }

    public async Task<TReturn?> GetObjectAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThird : class
        where TReturn : class
    {
        ThrowIfDisposed();

        var result = await Connection.QueryAsync(sql, map, param, transaction, options.Buffered, splitOn, options.CommandTimeout, commandType);
        return result.FirstOrDefault();
    }

    public async Task<TReturn?> GetObjectAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null, string splitOn = "Id")
        where TFirst : class
        where TSecond : class
        where TThird : class
        where TFourth : class
        where TReturn : class
    {
        ThrowIfDisposed();

        var result = await Connection.QueryAsync(sql, map, param, transaction, options.Buffered, splitOn, options.CommandTimeout, commandType);
        return result.FirstOrDefault();
    }

    public async Task<T?> GetSingleValueAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null)
    {
        ThrowIfDisposed();
        return await Connection.ExecuteScalarAsync<T>(sql, param, transaction, options.CommandTimeout, commandType);
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null)
    {
        ThrowIfDisposed();
        return await Connection.ExecuteAsync(sql, param, transaction, options.CommandTimeout, commandType);
    }

    public async Task<T> ExecuteAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CommandType? commandType = null) where T : class
    {
        ThrowIfDisposed();
        return await Connection.QuerySingleAsync<T>(sql, param, transaction, options.CommandTimeout, commandType);
    }

    public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
    {
        ThrowIfDisposed();
        return Connection.BeginTransaction(isolationLevel);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    if (connection.State is ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    connection.Dispose();
                    connection = null!;
                }
            }

            disposed = true;
        }
    }

    private async ValueTask DisposeAsync(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    if (connection.State is ConnectionState.Open)
                    {
                        await connection.CloseAsync();
                    }

                    await connection.DisposeAsync();
                    connection = null!;
                }
            }

            disposed = true;
        }
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(disposed, nameof(SqlClientContext));
    }
}