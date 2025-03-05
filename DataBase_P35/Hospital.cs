﻿using Hospital;
using MySql.Data.MySqlClient;
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
                ");";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

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

        public void RemoveDoctor(int id)
        {
            string query = "DELETE FROM Doctors WHERE ID=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public void EditDoctor(int id, Doctor update)
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
                command.ExecuteNonQuery();
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