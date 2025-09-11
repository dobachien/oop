using System;

namespace HW2
{
    internal class Person
    {
        private string name;
        private string address;
        private double salary;

        // Constructor
        public Person(string name, string address, double salary)
        {
            this.name = name;
            this.address = address;
            this.salary = salary;
        }

        // Getters/Setters
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public double Salary { get => salary; set => salary = value; }

        // Input method with validation
        public static Person InputPersonInfo()
        {
            Console.WriteLine("Input Information of Person");

            Console.Write("Please input name: ");
            string name = Console.ReadLine();

            Console.Write("Please input address: ");
            string address = Console.ReadLine();

            double salary = 0;
            while (true)
            {
                Console.Write("Please input salary: ");
                string sSalary = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(sSalary))
                {
                    Console.WriteLine("You must input Salary.");
                    continue;
                }

                if (!double.TryParse(sSalary, out salary))
                {
                    Console.WriteLine("You must input digit.");
                    continue;
                }

                if (salary <= 0)
                {
                    Console.WriteLine("Salary is greater than zero");
                    continue;
                }

                break;
            }

            return new Person(name, address, salary);
        }

        // Display person info
        public void DisplayPersonInfo()
        {
            Console.WriteLine("Information of Person you have entered:");
            Console.WriteLine($"Name: {name}");
            Console.WriteLine($"Address: {address}");
            Console.WriteLine($"Salary: {salary:F1}");
        }

        // Sort by salary (Bubble Sort)
        public static Person[] SortBySalary(Person[] persons)
        {
            if (persons == null || persons.Length == 0)
            {
                throw new Exception("Can't Sort Person");
            }

            for (int i = 0; i < persons.Length - 1; i++)
            {
                for (int j = 0; j < persons.Length - i - 1; j++)
                {
                    if (persons[j].Salary > persons[j + 1].Salary)
                    {
                        // Swap
                        Person temp = persons[j];
                        persons[j] = persons[j + 1];
                        persons[j + 1] = temp;
                    }
                }
            }
            return persons;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.InputEncoding=Encoding.UTF8;
            Console.OutputEncoding=Encoding.UTF8;
            Console.WriteLine("=====Management Person programer=====");

            // Nhập thông tin 3 Person
            Person[] people = new Person[3];
            for (int i = 0; i < 3; i++)
            {
                people[i] = Person.InputPersonInfo();
            }

            // Sắp xếp theo lương tăng dần
            people = Person.SortBySalary(people);

            // Hiển thị kết quả
            foreach (Person p in people)
            {
                p.DisplayPersonInfo();
            }

            Console.ReadKey();
        }
    }
}
