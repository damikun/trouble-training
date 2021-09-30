using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using FluentValidation;
using SharedCore.Aplication.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Persistence;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;
using Aplication.Payload;

namespace APIServer.Aplication.Commands.WebHooks {

    /// <summary>
    /// Command for updating webhook Uri
    /// </summary>
    [Authorize]
    public class UpdateWebHookTriggerEvents : IRequest<UpdateWebHookTriggerEventsPayload> {

        /// <summary>WebHook Id </summary>
        public long WebHookId { get; set; }

        /// <summary> HookEvents </summary>y>
        public HashSet<HookEventType> HookEvents { get; set; }
    }

    /// <summary>
    /// UpdateWebHookTriggerEvents Validator
    /// </summary>
    public class UpdateWebHookTriggerEventsValidator : AbstractValidator<UpdateWebHookTriggerEvents> {

        private readonly IDbContextFactory<ApiDbContext> _factory;

        public UpdateWebHookTriggerEventsValidator(IDbContextFactory<ApiDbContext> factory){
            _factory = factory;

            RuleFor(e => e.WebHookId)
            .NotNull()
            .GreaterThan(0);

            RuleFor(e => e.HookEvents)
            .NotNull();

            RuleFor(e => e.WebHookId)
            .MustAsync(HookExist)
            .WithMessage("Hook was not found");
        }

        public async Task<bool> HookExist(UpdateWebHookTriggerEvents request, long id, CancellationToken cancellationToken) {
    
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();
            
            return await dbContext.WebHooks.AnyAsync(e => e.ID == request.WebHookId);
        }
    }

    /// <summary>
    /// IUpdateWebHookTriggerEventsError
    /// </summary>
    public interface IUpdateWebHookTriggerEventsError { }

    /// <summary>
    /// UpdateWebHookTriggerEventsPayload
    /// </summary>
    public class UpdateWebHookTriggerEventsPayload : BasePayload<UpdateWebHookTriggerEventsPayload, IUpdateWebHookTriggerEventsError> {

        /// <summary>
        /// Updated WebHook
        /// </summary>
        public WebHook hook { get; set; }
    }

    /// <summary>Handler for <c>UpdateWebHookTriggerEvents</c> command </summary>
    public class UpdateWebHookTriggerEventsHandler : IRequestHandler<UpdateWebHookTriggerEvents, UpdateWebHookTriggerEventsPayload> {

        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Injected <c>IMediator</c>
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Injected <c>IMediator</c>
        /// </summary>
        private readonly ICurrentUser _current;

        /// <summary>
        /// Main constructor
        /// </summary>
        public UpdateWebHookTriggerEventsHandler(
            IDbContextFactory<ApiDbContext> factory,
            IMediator mediator,
            ICurrentUser currentuser) {

            _factory = factory;

            _mediator = mediator;

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>UpdateWebHookTriggerEvents</c>
        /// </summary>
        public async Task<UpdateWebHookTriggerEventsPayload> Handle(UpdateWebHookTriggerEvents request, CancellationToken cancellationToken) {

            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

            WebHook wh = await dbContext.WebHooks
            .TagWith(string.Format("UpdateWebHookTriggerEvents Command - Query Hook"))
            .Where(e => e.ID == request.WebHookId)
            .FirstOrDefaultAsync(cancellationToken);

            if (wh == null) {
                return UpdateWebHookTriggerEventsPayload.Error(new WebHookNotFound());
            }

            wh.HookEvents = request.HookEvents.Distinct().ToArray();

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = UpdateWebHookTriggerEventsPayload.Success();

            response.hook = wh;

            return response;
    }
    }
}