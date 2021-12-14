using MediatR;
using HotChocolate;
using HotChocolate.Types;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Aplication.Commands.WebHooks;

namespace APIServer.Aplication.GraphQL.Mutation
{

    /// <summary>
    /// WebHooks Mutations
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class WebHookMutations
    {

        /// <summary>
        /// Crate new  webhook
        /// </summary>
        public class CreateWebHookInput
        {

            /// <summary> Url </summary>
            public string WebHookUrl { get; set; }

            /// <summary> Secret </summary>
#nullable enable
            public string? Secret { get; set; }
#nullable disable

            /// <summary> IsActive </summary>
            public bool IsActive { get; set; }

            /// <summary> HookEvents </summary>
            public HookEventType[] HookEvents { get; set; }
        }

        /// <summary>
        /// Create new  webhook
        /// </summary>
        /// <returns></returns>
        public async Task<CreateWebHookPayload> CreateWebHook(
            CreateWebHookInput request,
            [Service] IMediator _mediator,
            [Service] IHttpContextAccessor accessor)
        {

            var id = accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _mediator.Send(new CreateWebHook()
            {
                WebHookUrl = request.WebHookUrl,
                Secret = request.Secret,
                IsActive = request.IsActive,
                HookEvents = request.HookEvents != null ? new HashSet<HookEventType>(request.HookEvents) : new HashSet<HookEventType>(new HookEventType[0]),
            });
        }

        /////////////////////////////////////////////////////
        /////////////////////////////////////////////////////

        /// <summary>
        /// Update  webhook
        /// </summary>
        public class UpdateWebHookInput
        {
            /// <summary>WebHook Id </summary>
            public long WebHookId { get; set; }

            /// <summary> Url </summary>
            public string WebHookUrl { get; set; }

            /// <summary> Secret </summary>

#nullable enable
            public string? Secret { get; set; }
#nullable disable

            /// <summary> IsActive </summary>
            public bool IsActive { get; set; }

            /// <summary> HookEvents </summary>
            public HookEventType[] HookEvents { get; set; }
        }

        /// <summary>
        /// Update existing  webhook
        /// </summary>
        /// <returns></returns>
        public async Task<UpdateWebHookPayload> UpdateWebHook(
            UpdateWebHookInput request,
            [Service] IMediator _mediator)
        {

            return await _mediator.Send(new UpdateWebHook()
            {
                WebHookId = request.WebHookId,
                WebHookUrl = request.WebHookUrl,
                Secret = request.Secret,
                IsActive = request.IsActive,
                HookEvents = request.HookEvents != null ? new HashSet<HookEventType>(request.HookEvents) : new HashSet<HookEventType>(new HookEventType[0]),
            });
        }

        /////////////////////////////////////////////////////
        /////////////////////////////////////////////////////

        /// <summary>
        /// Update webhook activ state Input
        /// </summary>
        public class UpdateWebHookActivStateInput
        {
            /// <summary>WebHook Id </summary>
            public long WebHookId { get; set; }

            /// <summary> IsActive </summary>
            public bool IsActive { get; set; }
        }

        /// <summary>
        /// Update webhook activ state
        /// </summary>
        /// <returns></returns>
        public async Task<UpdateWebHookActivStatePayload> UpdateWebHookActivState(
            UpdateWebHookActivStateInput request,
            [Service] IMediator _mediator)
        {

            return await _mediator.Send(new UpdateWebHookActivState()
            {
                WebHookId = request.WebHookId,
                IsActive = request.IsActive,
            });
        }

        /////////////////////////////////////////////////////
        /////////////////////////////////////////////////////

        /// <summary>
        /// Update webhook uri
        /// </summary>
        public class UpdateWebHookUriInput
        {
            /// <summary>WebHook Id </summary>
            public long WebHookId { get; set; }

            /// <summary>Webhook Uri </summary>
            public string WebHookUrl { get; set; }
        }

        /// <summary>
        /// Update webhook uri
        /// </summary>
        /// <returns></returns>
        public async Task<UpdateWebHookUriPayload> UpdateWebHookUri(
            UpdateWebHookUriInput request,
            [Service] IMediator _mediator)
        {

            return await _mediator.Send(new UpdateWebHookUri()
            {
                WebHookId = request.WebHookId,
                WebHookUrl = request.WebHookUrl,
            });
        }

        /////////////////////////////////////////////////////
        /////////////////////////////////////////////////////

        /// <summary>
        /// Update webhook secret
        /// </summary>
        public class UpdateWebHookSecretInput
        {
            /// <summary>WebHook Id </summary>
            public long WebHookId { get; set; }

            /// <summary>Webhook Secret </summary>
            public string Secret { get; set; }
        }

        /// <summary>
        /// Update webhook secret
        /// </summary>
        /// <returns></returns>
        public async Task<UpdateWebHookSecretPayload> UpdateWebHookSecret(
            UpdateWebHookSecretInput request,
            [Service] IMediator _mediator)
        {

            return await _mediator.Send(new UpdateWebHookSecret()
            {
                WebHookId = request.WebHookId,
                Secret = request.Secret,
            });
        }

        /////////////////////////////////////////////////////
        /////////////////////////////////////////////////////

        /// <summary>
        /// Update webhook triger events
        /// </summary>
        public class UpdateWebHookTriggerEventsInput
        {
            /// <summary>WebHook Id </summary>
            public long WebHookId { get; set; }

            /// <summary> HookEvents </summary>
            public HookEventType[] HookEvents { get; set; }
        }

        /// <summary>
        /// Update webhook triger events
        /// </summary>
        /// <returns></returns>
        public async Task<UpdateWebHookTriggerEventsPayload> UpdateWebHookTriggerEvents(
            UpdateWebHookTriggerEventsInput request,
            [Service] IMediator _mediator)
        {

            return await _mediator.Send(new UpdateWebHookTriggerEvents()
            {
                WebHookId = request.WebHookId,
                HookEvents = request.HookEvents != null ? new HashSet<HookEventType>(request.HookEvents) : new HashSet<HookEventType>(new HookEventType[0]),
            });
        }

        /////////////////////////////////////////////////////
        /////////////////////////////////////////////////////

        /// <summary>
        /// Update  webHook
        /// </summary>
        public class RemoveWebHookInput
        {
            /// <summary>WebHook Id </summary>
            public long WebHookId { get; set; }
        }

        /// <summary>
        /// Remove  WebHook
        /// </summary>
        /// <returns></returns>
        public async Task<RemoveWebHookPayload> RemoveWebHook(
            RemoveWebHookInput request,
            [Service] IMediator _mediator)
        {

            return await _mediator.Send(new RemoveWebHook()
            {
                WebHookId = request.WebHookId
            });
        }

    }
}