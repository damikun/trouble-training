using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.Interfaces {

    /// <summary>Main DBContext Interface </summary>
    public interface IApiDbContext : IDisposable, IAsyncDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable, IResettableService {

        DbSet<WebHook> WebHooks { get; set; }
        DbSet<WebHookRecord> WebHooksHistory { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DatabaseFacade Database { get; }
    }
}
