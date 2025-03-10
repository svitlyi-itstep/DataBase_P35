using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using Hospital;

class Program
{
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
                Console.WriteLine($"Підключення до бази даних {db_database}...");
                connection.Open();
                Console.WriteLine($"Успішно підключено до бази даних {db_database}");
                DBManager DataBase = new DBManager(connection);
                Console.WriteLine($"Оновлення таблиць...");
                DataBase.CreateTables();
                Console.WriteLine($"Таблиці оновлено!");
                Console.Write("Додати тестові записи у базу даних?\n (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    DataBase.AddTestData();
                }

                Console.Clear();
                List<Doctor> doctors = DataBase.GetDoctors();
                ShowDoctors(doctors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Environment.Exit(0);
        }
    }
}
