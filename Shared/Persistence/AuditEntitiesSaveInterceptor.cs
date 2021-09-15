using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Aplication.Interfaces;
using Shared.Domain.Models;

namespace Shared.Persistence {

    public class AuditEntitiesSaveInterceptor : SaveChangesInterceptor {
        private readonly ICurrentUser _currentUserService;

        public AuditEntitiesSaveInterceptor(ICurrentUser currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            AuditEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            AuditEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void AuditEntities(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _currentUserService.UserId != null ?_currentUserService.UserId?.ToString():"";
                    entry.Entity.CreatedUtc = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added ||
                    entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedBy =  _currentUserService.UserId != null ?_currentUserService.UserId?.ToString():"";
                    entry.Entity.LastModifiedUtc = DateTime.UtcNow;
                }
            }
        }
    }
}