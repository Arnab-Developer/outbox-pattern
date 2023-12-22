using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApplication2.Infra;

public class LogInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is StudentContext context)
        {
            var audit = new Audit() { Message = $"db operation {DateTime.Now}" };
            await context.Audits.AddAsync(audit, cancellationToken);
        }
        
        return result;
    }
}
