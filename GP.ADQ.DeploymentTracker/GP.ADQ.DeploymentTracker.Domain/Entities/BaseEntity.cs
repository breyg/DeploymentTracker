namespace GP.ADQ.DeploymentTracker.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
    }
}
