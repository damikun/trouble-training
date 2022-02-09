using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GreenDonut;
using APIServer.Persistence;
using APIServer.Aplication.GraphQL.DTO;
using SharedCore.Aplication.Interfaces;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace APIServer.Aplication.GraphQL.DataLoaders
{

    public class WebHookRecordByIdDataLoader : BatchDataLoader<long, GQL_WebHookRecord>
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

        private SemaphoreSlim _semaphoregate = new SemaphoreSlim(1);

        public WebHookRecordByIdDataLoader(
            IBatchScheduler scheduler,
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser current,
            IMapper mapper) : base(scheduler)
        {
            _mapper = mapper;

            _current = current;

            _factory = factory;
        }

        protected override async Task<IReadOnlyDictionary<long, GQL_WebHookRecord>> LoadBatchAsync(
            IReadOnlyList<long> keys,
            CancellationToken cancellationToken)
        {

            if (!_current.Exist)
            {
                return new List<GQL_WebHookRecord>().ToDictionary(e => e.ID, null);
            }

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            await _semaphoregate.WaitAsync();

            try
            {
                return await dbContext.WebHooksHistory
                .AsNoTracking()
                .Where(s => keys.Contains(s.ID))
                .ProjectTo<GQL_WebHookRecord>(_mapper.ConfigurationProvider)
                .ToDictionaryAsync(t => t.ID, cancellationToken);
            }
            finally
            {
                _semaphoregate.Release();
            }
        }
    }
}