using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqLeftJoin
{
    class Program
    {
        static void Main(string[] args)
        {
            var student = new List<Student>()
            {
                new Student() { Id = 1, Name = "Maria", AddressId = 1 },
                new Student() { Id = 1, Name = "Amelia", AddressId = 2 },
                new Student() { Id = 1, Name = "Rebecca" },
                new Student() { Id = 1, Name = "Una", AddressId = 3 },
                new Student() { Id = 1, Name = "Victoria", AddressId = 5 },
            };

            var address = new List<Address>()
            {
                new Address() { Id = 1, AddressLine = "Maria Address" },
                new Address() { Id = 2, AddressLine = "Amelia Address" },
                new Address() { Id = 3, AddressLine = "Una Address" },
            };

            var qs = (from std in student
                      join add in address on std.AddressId equals add.Id into stdAddress
                      from studentAddress in stdAddress.DefaultIfEmpty()
                      //where(std.Name.Contains("na"))
                      select new { std, studentAddress }).ToList();

            var qs2 = (from std in student
                      join add in address on std.AddressId equals add.Id into stdAddress
                      from studentAddress in stdAddress.DefaultIfEmpty()
                      select new { StudentName = std.Name, StudentAddress = studentAddress != null ? studentAddress.AddressLine : "N/A" }).ToList();

            var ms = student.GroupJoin(address, std => std.AddressId, addr => addr.Id, 
                (std, addr) => new { std, addr }).Where(x => x.std.Name.Contains("na"))
                .SelectMany(x => x.addr.DefaultIfEmpty(), 
                (studentData, addressData) => new 
                { 
                    studentData.std, 
                    addressData 
                })
                //.Where(x => x.addressData.AddressLine.Contains("na"))
                .ToList();

            var ms2 = student.GroupJoin(address, std => std.AddressId, addr => addr.Id,
                (std, addr) => new { std, addr })
                .SelectMany(x => x.addr.DefaultIfEmpty(),
                (studentData, addressData) => new
                {
                    StudnetName = studentData.std.Name,
                    StudentAddress = addressData?.AddressLine
                }).ToList();

            Console.WriteLine("Hello World!");
        }
    }

    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }
    }

    class Address
    {
        public int Id { get; set; }
        public string AddressLine { get; set; }
    }
}
