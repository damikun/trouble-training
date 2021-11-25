using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GreenDonut;
using SharedCore.Aplication.Interfaces;
using APIServer.Persistence;
using APIServer.Aplication.GraphQL.DTO;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.GraphQL.DataLoaders
{

    public class WebHookByIdDataLoader : BatchDataLoader<long, GQL_WebHook>
    {
        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Injected <c>ICurrentUser</c>
        /// </summary>
        private readonly ICurrentUser _current;

        private readonly SemaphoreSlim _semaphoregate = new SemaphoreSlim(1);

        public WebHookByIdDataLoader(
            IBatchScheduler scheduler,
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser current) : base(scheduler)
        {
            _current = current;
            _factory = factory;
        }

        protected override async Task<IReadOnlyDictionary<long, GQL_WebHook>> LoadBatchAsync(
            IReadOnlyList<long> keys,
            CancellationToken cancellationToken)
        {

            if (!_current.Exist)
            {
                return new List<GQL_WebHook>().ToDictionary(e => e.ID, null);
            }

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            await _semaphoregate.WaitAsync();

            try
            {
                return await dbContext.WebHooks
                .AsNoTracking()
                .Where(s => keys.Contains(s.ID))
                .Select(e => new GQL_WebHook
                {
                    ID = e.ID,
                    WebHookUrl = e.WebHookUrl,
                    // Secret = e.Secret,
                    ContentType = e.ContentType,
                    IsActive = e.IsActive,
                    LastTrigger = e.LastTrigger,
                    ListeningEvents = e.HookEvents != null ? e.HookEvents.ToArray() : new HookEventType[0]
                })
                .ToDictionaryAsync(t => t.ID, cancellationToken);
            }
            finally
            {
                _semaphoregate.Release();
            }
        }
    }
}