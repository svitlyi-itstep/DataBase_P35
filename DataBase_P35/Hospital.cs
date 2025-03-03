using Hospital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    class Doctor
    {

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

void Main(string[] args)
{
    List<Doctor> doctors = new List<Doctor>();
    doctors.Add(new Doctor());
}