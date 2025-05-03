namespace Net9WebAPI.DataAccess.DataModels;
public abstract class EntityBase
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
}