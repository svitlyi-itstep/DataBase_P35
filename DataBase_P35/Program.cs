using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using Hospital;

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

    static List<Doctor> GetDoctors(MySqlConnection connection)
    {
        string query = "SELECT * FROM Doctors";
        using(MySqlCommand command = new MySqlCommand(query, connection))
        using(MySqlDataReader reader = command.ExecuteReader())
        {
            List<Doctor> doctors = new List<Doctor>();
            while(reader.Read())
            {
                doctors.Add(new Doctor(
                        (int)reader["ID"],
                        (string)reader["Name"],
                        Convert.ToDecimal((double)reader["Premium"]),
                        Convert.ToDecimal((double)reader["Salary"])
                    ));
            }
            return doctors;
        }
    }

    static void ShowDoctors(IEnumerable<Doctor> doctors)
    {
        Console.WriteLine($"{"ID",3} | {"Прізвище та ім`я",20} | " +
            $"{"Премія",10} | {"Зарплатня",11}");
        foreach(Doctor doctor in doctors)
            Console.WriteLine($"{doctor.ID,3} | {doctor.Name,20} | " +
                        $"{doctor.Premium,5} грн. | {doctor.Salary,6} грн.");
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
                // AddDoctor(connection, "Петров Петро", 150, 1200);
                List<Doctor> doctors = GetDoctors(connection);
                ShowDoctors(doctors);
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
        }
    }
}
