namespace Domain.Exceptions
{
    
    
    
    
    public class DeviceValidationException : DomainException
    {
        public DeviceValidationException(string message) : base(message) { }

        public DeviceValidationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
