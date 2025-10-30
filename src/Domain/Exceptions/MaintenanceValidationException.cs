namespace Domain.Exceptions
{
    
    
    
    
    public class MaintenanceValidationException : DomainException
    {
        public MaintenanceValidationException(string message) : base(message) { }

        public MaintenanceValidationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
