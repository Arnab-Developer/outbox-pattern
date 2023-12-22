namespace WebApplication2.DomainEventHandlers;

public class ParentCreatedNotificationHandler(StudentContext context)
    : INotificationHandler<ParentCreatedNotification>
{
    private readonly StudentContext _context = context;

    public async Task Handle(ParentCreatedNotification notification,
        CancellationToken cancellationToken)
    {
        var intEvent = new IntEvent() { EventName = "Student and parent created" };
        await _context.IntEvents.AddAsync(intEvent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
