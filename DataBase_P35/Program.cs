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
                //DataBase.AddDoctor("Ніколаєнко Василь", 200, 1500);
                //List<Doctor> doctors = DataBase.GetDoctors();
                //ShowDoctors(doctors);
                //Doctor last_doctor = doctors.Last();

                //Console.ReadLine();
                //DataBase.EditDoctor(last_doctor.ID, 
                //    new Doctor(last_doctor.ID, "Григоренко Василь", 250, 1500));
                //doctors = DataBase.GetDoctors();
                //ShowDoctors(doctors);
                //last_doctor = doctors.Last();

                //Console.ReadLine();
                //DataBase.RemoveDoctor(last_doctor.ID);
                //doctors = DataBase.GetDoctors();
                //ShowDoctors(doctors);

                /*
                 
                Зробити інформаційну систему лікарів, яка одразу виводить
                таблицю з лікарями та меню, яке пропонує наступні дії:
                1. Додати лікаря
                2. Змінити лікаря
                3. Видалити лікаря
                0. Вихід

                В клас DBManager додати функцію, яка заповнює таблиці бази даних
                тестовими даними (мінімум 4 рядки таблиці).
                 
                 */

                /*
                
                Додати у клас DBManager функціонал для роботи з спеціалізаціями
                лікарів. 

                Додати 2 таблиці:
                - спеціалізації
                - спеціалізації в лікарів
                
                Додати функції для:
                - додавання/видалення/отримання спеціалізацій
                - додавання/видалення спеціалізацій у лікарів

                Додати при виведенні лікарів виведення їх спеціалізацій.
                 
                
                 */

                DataBase.AddSpecialization("Хірург");
                DataBase.AddSpecialization("Кардіолог");
                DataBase.AddSpecialization("Педіатр");


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Environment.Exit(0);
        }
    }
}
