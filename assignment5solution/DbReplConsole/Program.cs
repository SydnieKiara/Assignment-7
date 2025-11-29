using System;
using System.Threading.Tasks;
using DBConnector; // <- This is where IDBConnector, MongoConnector, PostgresConnector live

namespace DbReplConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Database Ping REPL ===");
            Console.WriteLine("This tool lets you test connections to MongoDB or PostgreSQL.\n");

            while (true)
            {
                Console.WriteLine("Choose a database type:");
                Console.WriteLine("  1) MongoDB");
                Console.WriteLine("  2) PostgreSQL");
                Console.WriteLine("  Q) Quit");
                Console.Write("Your choice: ");

                var choice = Console.ReadLine()?.Trim().ToLower();

                if (choice == "q")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }

                string dbType;
                switch (choice)
                {
                    case "1":
                    case "mongo":
                    case "mongodb":
                        dbType = "mongo";
                        break;

                    case "2":
                    case "postgres":
                    case "postgresql":
                        dbType = "postgres";
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please choose 1, 2, or Q.\n");
                        continue;
                }

                Console.Write("Enter connection string: ");
                var connectionString = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    Console.WriteLine("Connection string cannot be empty.\n");
                    continue;
                }

                IDBConnector connector;

                try
                {
                    connector = CreateConnector(dbType, connectionString);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating connector: {ex.Message}\n");
                    continue;
                }

                Console.WriteLine($"\nPinging {dbType} database...");
                try
                {
                    bool success = await connector.PingAsync(); // <-- if your interface still uses ping(), change this call

                    if (success)
                    {
                        Console.WriteLine("✅ Ping successful!\n");
                    }
                    else
                    {
                        Console.WriteLine("⚠️ Ping failed (PingAsync returned false).\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Ping threw an exception:");
                    Console.WriteLine(ex.Message + "\n");
                }
            }
        }

        private static IDBConnector CreateConnector(string dbType, string connectionString)
        {
            return dbType switch
            {
                "mongo" => new MongoConnector(connectionString),
                "postgres" => new PostgresConnector(connectionString),
                _ => throw new ArgumentOutOfRangeException(nameof(dbType), "Unsupported database type.")
            };
        }
    }
}
