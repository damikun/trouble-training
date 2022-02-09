using System;
using GreenDonut;
using AutoMapper;
using System.Linq;
using System.Threading;
using APIServer.Persistence;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.GraphQL.DTO;

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

        /// <summary>
        /// Injected <c>IMapper</c>
        /// </summary>
        private readonly IMapper _mapper;

        private readonly SemaphoreSlim _semaphoregate = new SemaphoreSlim(1);

        public WebHookByIdDataLoader(
            IBatchScheduler scheduler,
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser current,
            IMapper mapper) : base(scheduler)
        {
            _mapper = mapper;

            _current = current;

            _factory = factory;
        }

        protected override async Task<IReadOnlyDictionary<long, GQL_WebHook>> LoadBatchAsync(
            IReadOnlyList<long> keys,
            CancellationToken cancellationToken)
        {

            if (!_current.Exist)
            {
                return new List<GQL_WebHook>()
                .ToDictionary(e => e.ID, null);
            }

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            await _semaphoregate.WaitAsync();

            try
            {
                return await dbContext.WebHooks
                .AsNoTracking()
                .Where(s => keys.Contains(s.ID))
                .ProjectTo<GQL_WebHook>(_mapper.ConfigurationProvider)
                .ToDictionaryAsync(t => t.ID, cancellationToken);
            }
            finally
            {
                _semaphoregate.Release();
            }
        }
    }
}