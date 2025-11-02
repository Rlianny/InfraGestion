namespace Domain.Exceptions
{
    
    
    
    
    public class EntityNotFoundException : DomainException
    {
        public string EntityName { get; }
        public object EntityId { get; }

        public EntityNotFoundException(string entityName, object entityId) 
            : base($"{entityName} con ID '{entityId}' no fue encontrado")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public EntityNotFoundException(string entityName, object entityId, Exception innerException) 
            : base($"{entityName} con ID '{entityId}' no fue encontrado", innerException)
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
