using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Aggregations;
using Domain.Enums;

namespace Domain.Entities
{
    public class Technician : User
    {
        public Technician(string fullName, string passwordHash, int departmentId, int yearsOfExperience, string specialty, int salary)
            : base(fullName, passwordHash, departmentId)
        {
            ValidateSalary(salary);
            ValidateExperience(yearsOfExperience);
            Salary = salary;
            YearsOfExperience = yearsOfExperience;
            Specialty = specialty;
        }
        private Technician() : base()
        {
            Specialty = string.Empty;
        }
        public int Salary { get; private set; }
        public int YearsOfExperience { get; private set; }
        public string Specialty { get; private set; }

        private void ValidateExperience(int years)
        {
            if (years < 0)
            {
                throw new InvalidOperationException("Years of experience cannot be negative");
            }
        }
        public void UpdateSpecialty(string newSpecialty)
        {
            Specialty = newSpecialty;
        }
        public void UpdateExperience(int newYearsOfExperience)
        {
            ValidateExperience(newYearsOfExperience);
            YearsOfExperience = newYearsOfExperience;
        }
        private void ValidateSalary(int salary)
        {
            if (salary < 0)
            {
                throw new InvalidOperationException("Salary cannot be negative");
            }
        }
        public void UpdateSalary(int newSalary)
        {
            ValidateSalary(newSalary);
            Salary = newSalary;
        }

    }
}
