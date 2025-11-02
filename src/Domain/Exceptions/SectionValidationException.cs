namespace Domain.Exceptions
{
    
    
    
    
    public class SectionValidationException : DomainException
    {
        public SectionValidationException(string message) : base(message) { }

        public SectionValidationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
