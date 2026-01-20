namespace Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string EntityName { get; }

        public EntityNotFoundException(string entityName)
            : base($"{entityName} no fue encontrado.")
        {
            EntityName = entityName;
        }
    }
}
