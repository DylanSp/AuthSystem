namespace AuthSystem.Data
{
    public readonly struct Resource
    {
        public ResourceId Id { get; }
        public ResourceValue Value { get; }
        
        public Resource(ResourceId id, ResourceValue value)
        {
            Id = id;
            Value = value;
        }
    }
}
