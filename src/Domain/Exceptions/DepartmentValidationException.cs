namespace Domain.Exceptions
{
    
    
    
    
    public class DepartmentValidationException : DomainException
    {
        public DepartmentValidationException(string message) : base(message) { }

        public DepartmentValidationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
