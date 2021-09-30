using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using System.Net.Http;
using System.Diagnostics;
using SharedCore.Aplication.Interfaces;
using APIServer.Domain.Core.Models.Events;
using APIServer.Aplication.Commands.Internall.Hooks;
using APIServer.Aplication.WebHooks;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.Notifications.WebHooks {

    /// <summary>
    /// Notifi webhook created
    /// </summary>
    public class WebHookUpdatedNotifi : WebHookBaseNotifi {

    }

    /// <summary>
    /// Command handler for user <c>WebHookUpdatedNotifi</c>
    /// </summary>
    public class WebHookUpdatedEventLogHandler : INotificationHandler<WebHookUpdatedNotifi> {

        /// <summary>
        /// Injected <c>IMediator</c>
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Injected <c>ICurrentUser</c>
        /// </summary>
        private readonly ICurrentUser _currentuser;

        /// <summary>
        /// Injected <c>ILogger</c>
        /// </summary>
        private readonly ILogger _logger;

        public WebHookUpdatedEventLogHandler(
            IMediator mediator,
            ICurrentUser currentuser,
            ILogger logger,
            IHttpClientFactory clientFactory
            ) {

            _mediator = mediator;

            _currentuser = currentuser;

            _logger = logger;
        }

        /// <summary>
        /// Command handler for <c>WebHookUpdatedNotifi</c>
        /// </summary>
        public async Task Handle(WebHookUpdatedNotifi request, CancellationToken cancellationToken) {

            if (request == null )
                return;

            try {
                _mediator.Enqueue(new EnqueSaveEvent<WebHookUpdated>() {
                    Event = new WebHookUpdated() {
                        ActorID = _currentuser.UserId,
                        WebHookId = request.WebHookId,
                        TimeStamp = request.TimeStamp != default ? request.TimeStamp : DateTime.Now,
                    },
                    ActivityId = Activity.Current != null ? Activity.Current.Id : null
                });
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Enqueue IWebHookUpdatedEventLog");
            }

            return;
        }
    }


    /// <summary>
    /// Command handler for user <c>WebHookUpdatedNotifi</c>
    /// </summary>
    public class WebHookUpdatedHookQueueHandler : INotificationHandler<WebHookUpdatedNotifi> {

        /// <summary>
        /// Injected <c>IMediator</c>
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Injected <c>ICurrentUser</c>
        /// </summary>
        private readonly ICurrentUser _currentuser;

        /// <summary>
        /// Injected <c>ILogger</c>
        /// </summary>
        private readonly ILogger _logger;

        public WebHookUpdatedHookQueueHandler(
            IMediator mediator,
            ICurrentUser currentuser,
            ILogger logger,
            IHttpClientFactory clientFactory
            ) {

            _mediator = mediator;

            _currentuser = currentuser;

            _logger = logger;
        }

        /// <summary>
        /// Command handler for <c>WebHookUpdatedNotifi</c>
        /// </summary>
        public async Task Handle(WebHookUpdatedNotifi request, CancellationToken cancellationToken) {

            if (request == null )
                return;

            WebHookUpdated ev = new WebHookUpdated() {
                ActorID = _currentuser.UserId,
                WebHookId = request.WebHookId,
                TimeStamp = request.TimeStamp != default ? request.TimeStamp : DateTime.Now,
            };

            try {
                _mediator.Enqueue(new EnqueueRelatedWebHooks() {
                    Event =  new Hook_HookUpdated(
                                HookResourceAction.hook_updated,
                                new Hook_User_DTO() {
                                    id = ev?.ActorID?.ToString(),
                                    name = _currentuser.Name,
                                },
                                new Hook_HookUpdatedPayload() {}
                            ),
                    EventType = HookEventType.hook,
                });
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to Enqueue Related webhooks for WebHookUpdated");
            }

            return;
        }
    }
}