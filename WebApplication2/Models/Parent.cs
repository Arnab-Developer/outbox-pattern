namespace WebApplication2.Models;

public class Parent : Entity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public Parent()
    {
        Notifications.Add(new ParentCreatedNotification());
    }
}
