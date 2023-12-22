var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<LogInterceptor>();
builder.Services.AddHostedService<PublishIntEvent>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

var constr = builder.Configuration.GetConnectionString("Constr");

builder.Services.AddDbContext<StudentContext>((sp, options) =>
{
    var logInterceptor = sp.GetRequiredService<LogInterceptor>();
    options.UseSqlServer(constr).AddInterceptors(logInterceptor);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/weatherforecast", async (IMediator mediator, CancellationToken cancellationToken) =>
{
    var cmd = new CreateStudentCommand();
    await mediator.Send(cmd, cancellationToken);
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

/*
select * from students
select * from parents
select * from audits
select * from intevents

--delete from students
--delete from parents
--delete from audits
--delete from intevents
*/