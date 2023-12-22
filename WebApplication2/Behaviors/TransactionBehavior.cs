namespace WebApplication2.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(StudentContext context)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly StudentContext _context = context;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var tran = await _context.Database.BeginTransactionAsync(cancellationToken);
        var response = await next();
        await tran.CommitAsync(cancellationToken);
        return response;
    }
}
