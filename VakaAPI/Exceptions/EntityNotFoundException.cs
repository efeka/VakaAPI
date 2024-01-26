namespace Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base("Could not find entity.") { }
        public EntityNotFoundException(int entityId) : base($"Could not find entity with ID {entityId}.") { }
    }
}
