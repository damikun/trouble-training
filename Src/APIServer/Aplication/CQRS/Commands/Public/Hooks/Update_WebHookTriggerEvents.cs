using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using FluentValidation;
using MediatR.Pipeline;
using APIServer.Persistence;
using System.Threading.Tasks;
using System.Collections.Generic;
using SharedCore.Aplication.Payload;
using Microsoft.EntityFrameworkCore;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;
using APIServer.Domain.Core.Models.WebHooks;
using SharedCore.Aplication.Shared.Attributes;
using SharedCore.Aplication.Core.Commands;
using APIServer.Aplication.GraphQL.DTO;

namespace APIServer.Aplication.Commands.WebHooks
{

    /// <summary>
    /// Command for updating webhook Uri
    /// </summary>
    [Authorize]
    public class UpdateWebHookTriggerEvents : CommandBase<UpdateWebHookTriggerEventsPayload>
    {

        /// <summary>WebHook Id </summary>
        public long WebHookId { get; set; }

        /// <summary> HookEvents </summary>y>
        public HashSet<HookEventType> HookEvents { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// UpdateWebHookTriggerEvents Validator
    /// </summary>
    public class UpdateWebHookTriggerEventsValidator : AbstractValidator<UpdateWebHookTriggerEvents>
    {

        private readonly IDbContextFactory<ApiDbContext> _factory;

        public UpdateWebHookTriggerEventsValidator(IDbContextFactory<ApiDbContext> factory)
        {
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

        public async Task<bool> HookExist(UpdateWebHookTriggerEvents request, long id, CancellationToken cancellationToken)
        {

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            return await dbContext.WebHooks.AnyAsync(e => e.ID == request.WebHookId);
        }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// IUpdateWebHookTriggerEventsError
    /// </summary>
    public interface IUpdateWebHookTriggerEventsError { }

    /// <summary>
    /// UpdateWebHookTriggerEventsPayload
    /// </summary>
    public class UpdateWebHookTriggerEventsPayload : BasePayload<UpdateWebHookTriggerEventsPayload, IUpdateWebHookTriggerEventsError>
    {

        /// <summary>
        /// Updated WebHook
        /// </summary>
        public GQL_WebHook hook { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>Handler for <c>UpdateWebHookTriggerEvents</c> command </summary>
    public class UpdateWebHookTriggerEventsHandler : IRequestHandler<UpdateWebHookTriggerEvents, UpdateWebHookTriggerEventsPayload>
    {

        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Injected <c>IMapper</c>
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Injected <c>IMediator</c>
        /// </summary>
        private readonly ICurrentUser _current;

        /// <summary>
        /// Main constructor
        /// </summary>
        public UpdateWebHookTriggerEventsHandler(
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser currentuser,
            IMapper mapper)
        {
            _mapper = mapper;

            _factory = factory;

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>UpdateWebHookTriggerEvents</c>
        /// </summary>
        public async Task<UpdateWebHookTriggerEventsPayload> Handle(UpdateWebHookTriggerEvents request, CancellationToken cancellationToken)
        {

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            WebHook wh = await dbContext.WebHooks
            .TagWith(string.Format("UpdateWebHookTriggerEvents Command - Query Hook"))
            .Where(e => e.ID == request.WebHookId)
            .FirstOrDefaultAsync(cancellationToken);

            if (wh == null)
            {
                return UpdateWebHookTriggerEventsPayload.Error(new WebHookNotFound());
            }

            wh.HookEvents = request.HookEvents.Distinct().ToArray();

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = UpdateWebHookTriggerEventsPayload.Success();

            response.hook = _mapper.Map<GQL_WebHook>(wh); ;

            return response;
        }
    }

    //---------------------------------------
    //---------------------------------------

    public class UpdateWebHookTriggerEventsPostProcessor
        : IRequestPostProcessor<UpdateWebHookTriggerEvents, UpdateWebHookTriggerEventsPayload>
    {
        /// <summary>
        /// Injected <c>IPublisher</c>
        /// </summary>
        private readonly SharedCore.Aplication.Interfaces.IPublisher _publisher;

        public UpdateWebHookTriggerEventsPostProcessor(SharedCore.Aplication.Interfaces.IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Process(
            UpdateWebHookTriggerEvents request,
            UpdateWebHookTriggerEventsPayload response,
            CancellationToken cancellationToken)
        {
            if (response != null && !response.HasError())
            {
                try
                {

                    await Task.CompletedTask;

                    // Add Notification hire

                }
                catch { }
            }
        }
    }
}