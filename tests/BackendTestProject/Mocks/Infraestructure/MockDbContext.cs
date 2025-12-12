using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace BackendTestProject.Mocks.Infraestructure
{
    internal class MockDbContext : IUnitOfWork
    {
        private Dictionary<string, List<CommandRequest>> _commandsToExecute; 
        private Dictionary<string, Dictionary<int,object>> _databaseTables;

        private int GetIdValue(string tableName,object obj)
        {

            var type = obj.GetType();
            var idProperty = type.GetProperty("Id") ?? type.GetProperty($"{type.Name}Id");
            if (idProperty is null)
            {
                throw new Exception($"Entity{tableName} has not a valid Id property, create an Id or {tableName}Id property");
            }
            var idValue = idProperty.GetValue(obj);
            if (idValue is not null && idValue is int id)
            {
                return id;
            }
            throw new Exception($"Entity {tableName} has not an integer  Id value");
        }

        private int SetIdValue(string tableName,object obj)
        {

            var type = obj.GetType();
            var idProperty = (type.GetProperty("Id") ?? type.GetProperty($"{type.Name}Id")) ?? throw new Exception($"Entity{tableName} has not a valid Id property, create an Id or {tableName}Id property");
            int maxId = _databaseTables[tableName].Count > 0 ? _databaseTables[tableName].Keys.Max() : 0;
            try
            {
                idProperty.SetValue(obj, maxId + 1);
                return maxId + 1;
            }
            catch
            {
                throw new Exception($"Entity {tableName} has not an integer  Id value");
            }
        }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            bool succesfullOperation = true;
            foreach (var table in _commandsToExecute)
            {
                foreach (var command in _commandsToExecute)
                {
                    //TODO: Validate commands before executing them
                }
            }
            
            if (succesfullOperation)
            {
                foreach (var table in _commandsToExecute)
                {
                    foreach (var command in table.Value)
                    {
                        var tableName = table.Key;
                        switch (command.Type)
                        {
                            case CommandType.Add:
                               int id = SetIdValue(tableName, command.Object);
                               _databaseTables[tableName].Add(id,command.Object);
                               break;
                            case CommandType.Delete:
                                _databaseTables[tableName].Remove(GetIdValue(tableName, command.Object));
                                break;
                            case CommandType.Update:
                                _databaseTables[tableName].Remove(GetIdValue(tableName, command.Object));
                                id = SetIdValue(tableName, command.Object);
                                _databaseTables[tableName].Add(id, command.Object);
                                break;

                        }
                    }
                }

            }
            return Task.FromResult(succesfullOperation ? 1 : 0);
        }
    }
    internal struct  CommandRequest{
        public object Object { get; set; }  
        public CommandType Type { get; set; }
    }
    internal enum CommandType
    {
        Add,Delete,Update
    }
    
}
