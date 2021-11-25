using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Aplication.Interfaces
{

#pragma warning disable EF1001
    /// <summary>Main DBContext Interface </summary>
    public interface IAppDbContext : IDisposable, IAsyncDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable, IResettableService
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DatabaseFacade Database { get; }

    }
#pragma warning restore EF1001
}
