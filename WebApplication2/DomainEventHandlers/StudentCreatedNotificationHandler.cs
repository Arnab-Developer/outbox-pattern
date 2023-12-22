namespace WebApplication2.DomainEventHandlers;

public class StudentCreatedNotificationHandler(StudentContext context)
    : INotificationHandler<StudentCreatedNotification>
{
    private readonly StudentContext _context = context;

    public async Task Handle(StudentCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        var parent = new Parent() { Name = "parent 1" };
        await _context.Parents.AddAsync(parent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
