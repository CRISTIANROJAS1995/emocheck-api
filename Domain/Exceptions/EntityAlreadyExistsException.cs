namespace Domain.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public string EntityName { get; }

        public EntityAlreadyExistsException(string entityName)
            : base($"{entityName} ya existe.")
        {
            EntityName = entityName;
        }
    }
}
