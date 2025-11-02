namespace Domain.Exceptions
{
    
    
    
    
    
    public class AccessDeniedException : DomainException
    {
        public string? RequiredPermission { get; }
        public string? UserRole { get; }

        public AccessDeniedException(string message) : base(message) { }

        public AccessDeniedException(string message, string requiredPermission, string userRole) 
            : base(message)
        {
            RequiredPermission = requiredPermission;
            UserRole = userRole;
        }

        public AccessDeniedException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
