namespace Domain.Exceptions
{
    
    
    
    
    public class TransferValidationException : DomainException
    {
        public TransferValidationException(string message) : base(message) { }

        public TransferValidationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
