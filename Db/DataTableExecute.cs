using fotosmigracion.Db;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data;

namespace fotosmigracion.DT;

public class DataTableExecute : IDataTableExecute, IDisposable
{
    public bool ErrorStatus { get; private set; }
    public string? ErrorMsg { get; private set; }
    private readonly IConnectionFactory _connectionFactory;
    private bool _disposed = false;

    public DataTableExecute(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory
            ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public async Task<DataTable> ExecuteAsync(string storedProcedureName, Hashtable parameters)
    {
        ErrorStatus = false;
        ErrorMsg = null;

        var resultTable = new DataTable();

        await using (var connection = _connectionFactory.CreateConnection())
        await using (var command = new SqlCommand(storedProcedureName, connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 180;

            if (parameters != null)
            {
                foreach (DictionaryEntry entry in parameters)
                {
                    command.Parameters.AddWithValue(
                        entry.Key.ToString(),
                        entry.Value ?? DBNull.Value
                    );
                }
            }

            try
            {
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    resultTable.Load(reader);
                }

                ErrorStatus = true;
            }
            catch (SqlException ex)
            {
                ErrorStatus = false;
                ErrorMsg = $"SQL Error: {ex.Message}";
                Console.WriteLine(ErrorMsg);
            }
            catch (Exception ex)
            {
                ErrorStatus = false;
                ErrorMsg = $"General Error: {ex.Message}";
                Console.WriteLine(ErrorMsg);
            }
        }

        return resultTable;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }
}