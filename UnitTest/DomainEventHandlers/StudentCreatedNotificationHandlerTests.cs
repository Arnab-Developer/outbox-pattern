namespace UnitTest.DomainEventHandlers;

public class StudentCreatedNotificationHandlerTests
{
    private readonly Mock<StudentContext> _studentContextMock;
    private readonly StudentCreatedNotification _studentCreatedNotification;
    private readonly CancellationToken _cancellationToken;
    private readonly StudentCreatedNotificationHandler _studentCreatedNotificationHandler;

    public StudentCreatedNotificationHandlerTests()
    {
        _studentContextMock = new Mock<StudentContext>();
        _studentCreatedNotification = new StudentCreatedNotification();
        _cancellationToken = CancellationToken.None;
        _studentCreatedNotificationHandler = new StudentCreatedNotificationHandler(_studentContextMock.Object);
    }

    [Fact]
    public async Task Can_Handle_CreateProperParent()
    {
        _studentContextMock.SetupGet(s => s.Parents).Returns(It.IsAny<DbSet<Parent>>());
        _studentContextMock.Setup(s => s.Parents.AddAsync(It.IsAny<Parent>(), _cancellationToken));
        _studentContextMock.Setup(s => s.SaveChangesAsync(_cancellationToken));

        await _studentCreatedNotificationHandler.Handle(_studentCreatedNotification, _cancellationToken);

        _studentContextMock.VerifyGet(v => v.Parents, Times.Once);
        _studentContextMock.Verify(v => v.SaveChangesAsync(_cancellationToken), Times.Once);

        _studentContextMock
            .Verify(v => v.Parents.AddAsync(It.Is<Parent>(p => IsValidParent(p)), _cancellationToken),
                Times.Once);
    }

    private static bool IsValidParent(Parent parent) =>
        parent.Id == 0 &&
        parent.Name == "parent 1" &&
        parent.Notifications.Count == 1 &&
        parent.Notifications[0] is ParentCreatedNotification;
}
