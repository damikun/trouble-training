using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Aplication.Commands.Internall.Hooks  {

    /// <summary>
    /// Command for processing WebHook event
    /// </summary>
    public class EnqueueRelatedWebHooks : MediatR.IRequest {
        public HookEventType EventType { get; set; }
        public object Event { get; set; }
    }

    /// <summary>
    /// Command handler for <c>EnqueueRelatedWebHooks</c>
    /// </summary>
    public class EnqueueRelatedWebHooksHandler : IRequestHandler<EnqueueRelatedWebHooks, Unit> {

        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Injected <c>IMediator</c>
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Injected <c>IHttpClientFactory</c>
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Main Constructor
        /// </summary>
        public EnqueueRelatedWebHooksHandler(
            IDbContextFactory<ApiDbContext> factory,
            IMediator mediator,
            IHttpClientFactory httpClient) {
            _factory = factory;
            _mediator = mediator;
            _clientFactory = httpClient;
        }

        /// <summary>
        /// Command handler for  <c>EnqueueRelatedWebHooks</c>
        /// </summary>
        public async Task<Unit> Handle(EnqueueRelatedWebHooks request, CancellationToken cancellationToken) {
            
            if (request == null || request.Event == null) {
                throw new ArgumentNullException();
            }

            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

            List<WebHook> hooks = await dbContext.WebHooks
            .AsNoTracking()
            .Where(e => e.HookEvents.Contains(HookEventType.hook))
            .ToListAsync(cancellationToken);

            // try {
                if (hooks != null) {
                    foreach (var hook_item in hooks) {
                        if (hook_item.IsActive && hook_item.ID > 0) {

                    
                                _mediator.Enqueue(new ProcessWebHook() {
                                    HookId = hook_item.ID,
                                    Event = request.Event,
                                    EventType = request.EventType
                                });
                     
                        }
                    }
                }
            // } catch { }


            return Unit.Value;
        }
    }
}