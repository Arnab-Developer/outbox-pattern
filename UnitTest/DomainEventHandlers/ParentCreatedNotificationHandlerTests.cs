namespace UnitTest.DomainEventHandlers;

public class ParentCreatedNotificationHandlerTests
{
    private readonly Mock<StudentContext> _studentContextMock;
    private readonly ParentCreatedNotification _parentCreatedNotification;
    private readonly CancellationToken _cancellationToken;
    private readonly ParentCreatedNotificationHandler _parentCreatedNotificationHandler;

    public ParentCreatedNotificationHandlerTests()
    {
        _studentContextMock = new Mock<StudentContext>();
        _parentCreatedNotification = new ParentCreatedNotification();
        _cancellationToken = CancellationToken.None;
        _parentCreatedNotificationHandler = new ParentCreatedNotificationHandler(_studentContextMock.Object);
    }

    [Fact]
    public async Task Can_Handle_CreateProperIntEvent()
    {
        _studentContextMock.SetupGet(s => s.IntEvents).Returns(It.IsAny<DbSet<IntEvent>>());
        _studentContextMock.Setup(s => s.IntEvents.AddAsync(It.IsAny<IntEvent>(), _cancellationToken));
        _studentContextMock.Setup(s => s.SaveChangesAsync(_cancellationToken));

        await _parentCreatedNotificationHandler.Handle(_parentCreatedNotification, _cancellationToken);

        _studentContextMock.VerifyGet(v => v.IntEvents, Times.Once);
        _studentContextMock.Verify(v => v.SaveChangesAsync(_cancellationToken), Times.Once);

        _studentContextMock
            .Verify(v => v.IntEvents.AddAsync(It.Is<IntEvent>(e => IsValidIntEvent(e)), _cancellationToken),
                Times.Once);
    }

    private static bool IsValidIntEvent(IntEvent intEvent) =>
        intEvent.Id == 0 &&
        intEvent.EventName == "Student and parent created";
}
