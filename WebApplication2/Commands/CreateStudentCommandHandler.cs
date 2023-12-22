namespace WebApplication2.Commands;

public class CreateStudentCommandHandler(StudentContext context)
    : IRequestHandler<CreateStudentCommand, bool>
{
    private readonly StudentContext _context = context;

    public async Task<bool> Handle(CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var student = new Student() { Name = "jon 1" };
        await _context.Students.AddAsync(student, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
