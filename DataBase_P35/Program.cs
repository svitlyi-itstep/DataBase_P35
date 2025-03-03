using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;

class Program
{
    static void CreateTables(MySqlConnection connection)
    {
        string query = 
            "CREATE TABLE IF NOT EXISTS Doctors (" +
            "   ID INT AUTO_INCREMENT PRIMARY KEY," +
            "   Name VARCHAR(100)," +
            "   Premium DOUBLE," +
            "   Salary DOUBLE" +
            ");";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    static void AddDoctor(MySqlConnection connection, string name, double premium, double salary)
    {
        string query =
            "INSERT INTO Doctors (Name, Premium, Salary) VALUES (@name, @premium, @salary)";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@premium", premium);
            command.Parameters.AddWithValue("@salary", salary);
            command.ExecuteNonQuery();
        }
    }

    static void GetDoctors(MySqlConnection connection)
    {
        string query = "SELECT * FROM Doctors";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        using(MySqlDataReader reader = command.ExecuteReader())
        {
            while(reader.Read())
            {
                Console.WriteLine($"{reader["ID"], 3} | {reader["Name"], 20} | " +
                    $"{reader["Premium"], 5} грн. | {reader["Salary"], 6} грн.");
            }
        }
    }
    
    public static void Main(string[] args)
    {
        Console.OutputEncoding = UTF8Encoding.UTF8;
        Console.InputEncoding = UTF8Encoding.UTF8;
        string db_host = "localhost";
        string db_database = "hospital";
        string db_user = "root";
        string db_password = "";
        
        string connectionString = $"Server={db_host};Database={db_database};" +
            $"User ID={db_user};Password={db_password};";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            { 
                connection.Open();
                CreateTables(connection);
                AddDoctor(connection, "Петров Петро", 150, 1200);
                GetDoctors(connection);
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
        }
    }
}
