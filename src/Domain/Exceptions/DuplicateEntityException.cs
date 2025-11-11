namespace Domain.Exceptions
{
    
    
    
    
    public class DuplicateEntityException : DomainException
    {
        public string EntityName { get; }
        public string FieldName { get; }
        public object FieldValue { get; }

        public DuplicateEntityException(string entityName, string fieldName, object fieldValue) 
            : base($"{entityName} con {fieldName} '{fieldValue}' ya existe")
        {
            EntityName = entityName;
            FieldName = fieldName;
            FieldValue = fieldValue;
        }

        public DuplicateEntityException(string message) : base(message) { }
    }
}
