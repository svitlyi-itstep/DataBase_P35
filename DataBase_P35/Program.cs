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
                connection.Open();

                DBManager DataBase = new DBManager(connection);

                DataBase.CreateTables();
                DataBase.AddDoctor("Ніколаєнко Василь", 200, 1500);
                List<Doctor> doctors = DataBase.GetDoctors();
                ShowDoctors(doctors);
                Doctor last_doctor = doctors.Last();

                Console.ReadLine();
                DataBase.EditDoctor(last_doctor.ID, new Doctor(last_doctor.ID, "Григоренко Василь", 250, 1500));
                doctors = DataBase.GetDoctors();
                ShowDoctors(doctors);
                last_doctor = doctors.Last();

                Console.ReadLine();
                DataBase.RemoveDoctor(last_doctor.ID);
                doctors = DataBase.GetDoctors();
                ShowDoctors(doctors);

            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
        }
    }
}
