namespace Domain.Exceptions
{
    
    
    
    
    public class RoleValidationException : DomainException
    {
        public RoleValidationException(string message) : base(message) { }

        public RoleValidationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
