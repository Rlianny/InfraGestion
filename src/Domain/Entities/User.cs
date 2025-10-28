using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Domain.Entities
{
    public class User
    {
        public User(string fullName, string password, int departmentID)
        {
            ValidateName(fullName);
            ValidatePassword(password);
            ValidateDepartment(departmentID);
            FullName = fullName;
            PasswordHash = GetHash(password);
            DepartmentID = departmentID;

        }
        protected User()
        {
            FullName = string.Empty;
            PasswordHash = string.Empty;
        }

        public int UserID { get; private set; }
        public string FullName { get; private set; }
        public string PasswordHash { get; private set; }
        public int DepartmentID { get; private set; }
        public bool IsActive { get; private set; }

        public void ChangeDepartment(int newDepartmentId)
        {
            if (newDepartmentId >= 0)
            {
                DepartmentID = newDepartmentId;
                return;
            }
            throw new InvalidOperationException();
        }
        private void ValidateDepartment(int depId)
        {
            if (depId < 0)
            {
                throw new Exception("Department ID cannot be lower than 0");
            }
        }
        private void ValidateName(string name)
        {
            if (name == string.Empty || name.Length < 3)
            {
                throw new InvalidOperationException("Cannot set this name because is to small");
            }
        }
        private void ValidatePassword(string password)
        {
            if (password == string.Empty || password.Length < 3)
            {
                throw new InvalidOperationException("Cannot set this password because is to small");
            }
        }
        private string GetHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public void ChangePassword(string password)
        {
            ValidatePassword(password);
            PasswordHash = GetHash(password);
        }
        public void Deactivate()
        {
            IsActive = false;
        }
        public void Activate()
        {
            IsActive = true;
        }
    }
}
