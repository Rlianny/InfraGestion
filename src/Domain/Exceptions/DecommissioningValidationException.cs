namespace Domain.Exceptions
{
    
    
    
    
    public class DecommissioningValidationException : DomainException
    {
        public DecommissioningValidationException(string message) : base(message) { }

        public DecommissioningValidationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
