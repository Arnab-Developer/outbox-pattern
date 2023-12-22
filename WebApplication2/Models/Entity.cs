namespace WebApplication2.Models;

public abstract class Entity
{
    public IList<INotification> Notifications { get; set; } = new List<INotification>();
}