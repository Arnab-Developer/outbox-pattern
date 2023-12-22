namespace WebApplication2.Infra;

public class StudentContext : DbContext
{
    private readonly IMediator? _mediator;

    public StudentContext()
    {
    }

    public StudentContext(DbContextOptions<StudentContext> options)
        : base(options)
    {
    }

    public StudentContext(DbContextOptions<StudentContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Parent> Parents { get; set; }
    public virtual DbSet<Audit> Audits { get; set; }
    public virtual DbSet<IntEvent> IntEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().Ignore(s => s.Notifications);
        modelBuilder.Entity<Parent>().Ignore(s => s.Notifications);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_mediator is not null)
        {
            var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.Notifications.Clear());

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
