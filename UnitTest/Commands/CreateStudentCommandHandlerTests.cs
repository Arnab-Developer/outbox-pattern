namespace UnitTest.Commands;

public class CreateStudentCommandHandlerTests
{
    private readonly Mock<StudentContext> _studentContextMock;
    private readonly CreateStudentCommand _createStudentCommand;
    private readonly CancellationToken _cancellationToken;
    private readonly CreateStudentCommandHandler _createStudentCommandHandler;

    public CreateStudentCommandHandlerTests()
    {
        _studentContextMock = new Mock<StudentContext>();
        _createStudentCommand = new CreateStudentCommand();
        _cancellationToken = CancellationToken.None;
        _createStudentCommandHandler = new CreateStudentCommandHandler(_studentContextMock.Object);
    }

    [Fact]
    public async Task Can_Handle_CreateProperStudent()
    {
        _studentContextMock.SetupGet(s => s.Students).Returns(It.IsAny<DbSet<Student>>());
        _studentContextMock.Setup(s => s.Students.AddAsync(It.IsAny<Student>(), _cancellationToken));
        _studentContextMock.Setup(s => s.SaveChangesAsync(_cancellationToken));

        var isSuccess = await _createStudentCommandHandler.Handle(_createStudentCommand, _cancellationToken);
        Assert.True(isSuccess);

        _studentContextMock.VerifyGet(v => v.Students, Times.Once);
        _studentContextMock.Verify(v => v.SaveChangesAsync(_cancellationToken), Times.Once);

        _studentContextMock
            .Verify(v => v.Students.AddAsync(It.Is<Student>(s => IsValidStudent(s)), _cancellationToken),
                Times.Once);
    }

    private static bool IsValidStudent(Student student) =>
        student.Id == 0 &&
        student.Name == "jon 1" &&
        student.Notifications.Count == 1 &&
        student.Notifications[0] is StudentCreatedNotification;
}
