namespace WebApplication2.Models;

public class Student : Entity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public Student()
    {
        Notifications.Add(new StudentCreatedNotification());
    }
}
