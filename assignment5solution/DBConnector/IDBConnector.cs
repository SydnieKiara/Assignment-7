namespace DBConnector;

public interface IDBConnector
{
    Task<bool> PingAsync();
}

