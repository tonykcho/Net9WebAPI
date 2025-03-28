namespace Net9WebAPI.Domain.Abstract;
public abstract class BaseModel
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastUpdatedAt { get; set; }
}