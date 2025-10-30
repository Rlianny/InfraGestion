namespace Domain.Exceptions
{
    
    
    
    
    public class InvalidStateTransitionException : DomainException
    {
        public string EntityName { get; }
        public string CurrentState { get; }
        public string AttemptedState { get; }

        public InvalidStateTransitionException(
            string entityName, 
            string currentState, 
            string attemptedState) 
            : base($"No se puede cambiar {entityName} de estado '{currentState}' a '{attemptedState}'")
        {
            EntityName = entityName;
            CurrentState = currentState;
            AttemptedState = attemptedState;
        }
    }
}
