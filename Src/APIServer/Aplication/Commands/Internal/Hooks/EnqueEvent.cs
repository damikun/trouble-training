using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using SharedCore.Aplication.Core.Commands;
using Microsoft.EntityFrameworkCore;
using APIServer.Persistence;

namespace APIServer.Aplication.Commands.Internall.Hooks
{

    /// <summary>
    /// Command for saving WebHookCreated
    /// </summary>
    public class EnqueSaveEvent<T> : CommandBase
    {

        public T Event { get; set; }
    }

    /// <summary>
    /// Command handler for <c>EnqueSaveEvent</c>
    /// </summary>
    public class EnqueSaveEventHandler<T> : IRequestHandler<EnqueSaveEvent<T>, Unit>
    {

        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Main Constructor
        /// </summary>
        public EnqueSaveEventHandler(IDbContextFactory<ApiDbContext> factory)
        {

            _factory = factory;
        }

        /// <summary>
        /// Command handler for  <c>EnqueSaveEvent</c>
        /// </summary>
        public async Task<Unit> Handle(EnqueSaveEvent<T> request, CancellationToken cancellationToken)
        {

            if (request == null || request.Event == null)
            {
                throw new ArgumentNullException();
            }

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            dbContext.Add(request.Event);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

}