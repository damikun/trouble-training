using MediatR;
using System.Threading;
using System.Threading.Tasks;
using APIServer.Persistence;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SharedCore.Aplication.Core.Commands;

namespace APIServer.Aplication.Commands.Internall
{
    /////////////////////////////////////////
    // This is TEST only internall command
    /////////////////////////////////////////


    // This is TEST only internall command
    /// <summary>
    /// Command for resetting database
    /// </summary>
    public class ResetDatabase : CommandBase
    {

    }

    /// <summary>
    /// Command handler for <c>ResetDatabase</c>
    /// </summary>
    public class ResetDatabaseHandler : IRequestHandler<ResetDatabase, Unit>
    {

        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Injected <c>IWebHostEnvironment</c>
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Main Constructor
        /// </summary>
        public ResetDatabaseHandler(
            IDbContextFactory<ApiDbContext> factory,
            IWebHostEnvironment env)
        {
            _factory = factory;

            _env = env;
        }

        /// <summary>
        /// Command handler for <c>ResetDatabase</c>
        /// </summary>
        public async Task<Unit> Handle(ResetDatabase request, CancellationToken cancellationToken)
        {
            if (!_env.IsProduction())
            {
                await using ApiDbContext dbContext =
                    _factory.CreateDbContext();

                // Clear all Webhooks
                if (await dbContext.WebHooks.AnyAsync())
                {
                    dbContext.WebHooks.RemoveRange(dbContext.WebHooks);

                    await dbContext.SaveChangesAsync();
                }

                // Clear all WebHooksHistory
                if (await dbContext.WebHooksHistory.AnyAsync())
                {
                    dbContext.WebHooksHistory.RemoveRange(dbContext.WebHooksHistory);

                    await dbContext.SaveChangesAsync();
                }

                // Clear all Events
                if (await dbContext.Events.AnyAsync())
                {
                    dbContext.Events.RemoveRange(dbContext.Events);

                    await dbContext.SaveChangesAsync();
                }
            }

            return Unit.Value;
        }
    }
}