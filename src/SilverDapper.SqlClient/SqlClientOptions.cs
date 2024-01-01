namespace SilverDapper.SqlClient;

public class SqlClientOptions
{
    public string ConnectionString { get; set; } = null!;

    public int? CommandTimeout { get; set; }

    public bool Buffered { get; set; } = true;
}