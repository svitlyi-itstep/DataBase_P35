using Hospital;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    class DBManager
    {
        MySqlConnection connection;

        public DBManager(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public void CreateTables()
        {
            string query =
                "CREATE TABLE IF NOT EXISTS Doctors (" +
                "   ID INT AUTO_INCREMENT PRIMARY KEY," +
                "   Name VARCHAR(100)," +
                "   Premium DOUBLE," +
                "   Salary DOUBLE" +
                ");" +

                "CREATE TABLE IF NOT EXISTS Specializations (" +
                "   ID INT AUTO_INCREMENT PRIMARY KEY," +
                "   Name VARCHAR(100)" +
                ");" +

                "CREATE TABLE IF NOT EXISTS DoctorsSpecializations (" +
                "   ID INT AUTO_INCREMENT PRIMARY KEY," +
                "   DoctorID INT," +
                "   SpecializationID INT," +
                "   FOREIGN KEY (DoctorID) REFERENCES Doctors(ID)," +
                "   FOREIGN KEY (SpecializationID) REFERENCES Specializations(ID)" +
                ");";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void AddTestData()
        {
            AddDoctor("Григоренко Василь", 230, 1540);
            AddDoctor("Петров Дмитро", 160, 1300);
            AddDoctor("Ніколаєнко Захар", 200, 1250);
            AddDoctor("Дейнега Кирило", 90, 1800);
            
            AddSpecialization("Хірург");
            AddSpecialization("Кардіолог");
            AddSpecialization("Педіатр");

            Random rnd = new Random();
            List<Specialization> specializations = GetSpecializations();
            List<Doctor> doctors = GetDoctors();

            foreach (Doctor doctor in doctors)
            {
                for (int i = 0; i < rnd.Next(1, 3); i++)
                {
                    int specId = specializations[rnd.Next(0, specializations.Count)].ID;
                    if (!IsDoctorHaveSpecialization(doctor.ID, specId))
                        AddDoctorSpecialization(doctor.ID, specId);
                }
            }
        }

        // == SPECIALIZATIONS

        public void AddSpecialization(string name)
        {
            string query =
                "INSERT INTO Specializations (Name) VALUES (@name)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();
            }
        }

        public int RemoveSpecialization(int id)
        {
            string query = "DELETE FROM Specializations WHERE ID=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return command.ExecuteNonQuery();
            }
        }

        public List<Specialization> GetSpecializations()
        {
            string query = "SELECT * FROM Specializations";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                List<Specialization> specializations = new List<Specialization>();
                while (reader.Read())
                {
                    specializations.Add(
                        new Specialization(
                            (int)reader["ID"],
                            (string)reader["Name"]
                        ));
                }
                return specializations;
            }
        }

        // == DOCTORS SPECIALIZATIONS

        public void AddDoctorSpecialization(int doctorID, int specializationID)
        {
            string query =
                "INSERT INTO DoctorsSpecializations (DoctorID, SpecializationID)" +
                " VALUES (@doctorID, @specializationID)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@doctorID", doctorID);
                command.Parameters.AddWithValue("@specializationID", specializationID);
                command.ExecuteNonQuery();
            }
        }

        public int RemoveDoctorSpecialization(int doctorID, int specializationID)
        {
            string query = "DELETE FROM DoctorsSpecializations" +
                " WHERE DoctorID=@doctorID AND SpecializationID=@specializationID";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@doctorID", doctorID);
                command.Parameters.AddWithValue("@specializationID", specializationID);
                return command.ExecuteNonQuery();
            }
        }

        public bool IsDoctorHaveSpecialization(int doctorID, int specializationID)
        {
            string query = "SELECT ID FROM DoctorsSpecializations" +
                " WHERE DoctorID=@doctorID AND SpecializationID=@specializationID";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@doctorID", doctorID);
                command.Parameters.AddWithValue("@specializationID", specializationID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        // == DOCTORS

        public void AddDoctor(string name, double premium, double salary)
        {
            string query =
                "INSERT INTO Doctors (Name, Premium, Salary) VALUES (@name, @premium, @salary)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@premium", premium);
                command.Parameters.AddWithValue("@salary", salary);
                command.ExecuteNonQuery();
            }
        }

        public List<Doctor> GetDoctors()
        {
            string query = "SELECT * FROM Doctors";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                List<Doctor> doctors = new List<Doctor>();
                while (reader.Read())
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

        public int RemoveDoctor(int id)
        {
            string query = "DELETE FROM Doctors WHERE ID=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return command.ExecuteNonQuery();
            }
        }

        public int EditDoctor(int id, Doctor update)
        {
            string query = "UPDATE Doctors " +
                "SET ID=@id, Name=@name, Premium=@premium, Salary=@salary " +
                "WHERE ID=@source_id;";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@source_id", id);
                command.Parameters.AddWithValue("@id", update.ID);
                command.Parameters.AddWithValue("@name", update.Name);
                command.Parameters.AddWithValue("@premium", update.Premium);
                command.Parameters.AddWithValue("@salary", update.Salary);
                return command.ExecuteNonQuery();
            }
        }
    }

    class Doctor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Premium { get; set; }
        public decimal Salary { get; set; }

        public Doctor(int id, string name, 
            decimal premium, decimal salary)
        {
            ID = id;
            Name = name;
            Premium = premium;
            Salary = salary;
        }
        public Doctor(string name,
            decimal premium, decimal salary)
            : this(0, name, premium, salary) { }
        public Doctor() : this(0, "", 0, 0) { }
    }

    class Specialization
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Specialization(int id, string name)
        {
            ID = id;
            Name = name;
        }
        public Specialization(string name)
            : this(0, name) { }
        public Specialization() : this(0, "") { }
    }
}

/*
    1. Створити клас Doctor, в якому зберігається
        інформація про лікаря
    2. Змінити метод GetDoctors таким чином, 
        щоб він не виводив таблицю з лікарями, а
        повертав список об`єктів класу Doctor.
        Список лікарів — List<Doctor>
    3. Створити функцію ShowDoctors, яка приймає
        список лікарів та виводить його на екран
        у вигляді таблиці. Реалізувати також
        шапку таблиці.

*/