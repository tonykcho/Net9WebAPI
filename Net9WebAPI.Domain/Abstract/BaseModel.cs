namespace Net9WebAPI.Domain.Abstract;
public abstract class BaseModel
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}