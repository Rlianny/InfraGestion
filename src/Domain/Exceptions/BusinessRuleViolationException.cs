namespace Domain.Exceptions
{
    
    
    
    
    public class BusinessRuleViolationException : DomainException
    {
        public string RuleName { get; }

        public BusinessRuleViolationException(string ruleName, string message) 
            : base(message)
        {
            RuleName = ruleName;
        }

        public BusinessRuleViolationException(string message) : base(message) 
        {
            RuleName = "Unknown";
        }
    }
}
