using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIServer.Aplication.GraphQL.DTO;
using SharedCore.Aplication.Interfaces;
using IdentityServer.Persistence;
using GreenDonut;

namespace Aplication.GraphQL.DataLoaders
{

    public class UserByIdDataLoader : BatchDataLoader<string, GQL_User>
    {

        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<AppIdnetityDbContext> _factory;

        /// <summary>
        /// Injected <c>ICurrentUser</c>
        /// </summary>
        private readonly ICurrentUser _current;

        private SemaphoreSlim _semaphoregate = new SemaphoreSlim(1);

        public UserByIdDataLoader(
            IBatchScheduler scheduler,
            IDbContextFactory<AppIdnetityDbContext> factory,
            ICurrentUser current) : base(scheduler)
        {
            _current = current;
            _factory = factory;
        }

        protected override async Task<IReadOnlyDictionary<string, GQL_User>> LoadBatchAsync(
            IReadOnlyList<string> keys,
            CancellationToken cancellationToken)
        {

            if (!_current.Exist)
            {
                return new List<GQL_User>().ToDictionary(e => e.Guid, null);
            }

            await _semaphoregate.WaitAsync();

            await using AppIdnetityDbContext dbContext =
                _factory.CreateDbContext();

            try
            {
                return await dbContext.Users
                .AsNoTracking()
                .Where(s => keys.Contains(s.Id))
                .Select(e => new GQL_User
                {
                    Guid = e.Id,
                    Name = e.UserName,
                }).ToDictionaryAsync(t => t.Guid, cancellationToken);

            }
            finally
            {
                _semaphoregate.Release();
            }

        }
    }
}
