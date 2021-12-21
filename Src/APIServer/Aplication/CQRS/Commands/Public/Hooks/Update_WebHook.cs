using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using FluentValidation;
using SharedCore.Aplication.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Aplication.Shared;
using APIServer.Persistence;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;
using APIServer.Aplication.Notifications.WebHooks;
using System.Diagnostics;
using SharedCore.Aplication.Payload;
using MediatR.Pipeline;
using SharedCore.Aplication.Core.Commands;
using SharedCore.Aplication.Services;
using APIServer.Aplication.GraphQL.DTO;
using AutoMapper;

namespace APIServer.Aplication.Commands.WebHooks
{

    /// <summary>
    /// Command for updating webhook
    /// </summary>
    [Authorize]
    public class UpdateWebHook : CommandBase<UpdateWebHookPayload>
    {

        public UpdateWebHook()
        {
            this.HookEvents = new HashSet<HookEventType>();
        }

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
        public HashSet<HookEventType> HookEvents { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// UpdateWebHook Validator
    /// </summary>
    public class UpdateWebHookValidator : AbstractValidator<UpdateWebHook>
    {

        private readonly IDbContextFactory<ApiDbContext> _factory;

        public UpdateWebHookValidator(IDbContextFactory<ApiDbContext> factory)
        {
            _factory = factory;

            RuleFor(e => e.WebHookId)
            .NotNull()
            .GreaterThan(0);

            RuleFor(e => e.WebHookUrl)
            .NotEmpty()
            .NotNull();

            RuleFor(e => e.WebHookUrl)
            .Matches(Common.URI_REGEX)
            .WithMessage("Does not match URI expression");

            RuleFor(e => e.HookEvents)
            .NotNull();

            RuleFor(e => e.WebHookUrl)
            .MustAsync(async (command, url, cancellation) => await BeUniqueByURL(
               url, command.WebHookId, cancellation)
            ).WithMessage("Hook endpoint allready exist");

            RuleFor(e => e.WebHookId)
            .MustAsync(HookExist)
            .WithMessage("Hook was not found");
        }

        public async Task<bool> BeUniqueByURL(
            string url,
            long hook_id,
            CancellationToken cancellationToken)
        {

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            return !await dbContext.WebHooks.AnyAsync(e => e.WebHookUrl == url && e.ID != hook_id);
        }

        public async Task<bool> HookExist(long hook_id, CancellationToken cancellationToken)
        {

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            return (await dbContext.WebHooks.AnyAsync(e => e.ID == hook_id));
        }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// IUpdateWebHookError
    /// </summary>
    public interface IUpdateWebHookError { }

    /// <summary>
    /// UpdateWebHookPayload
    /// </summary>
    public class UpdateWebHookPayload : BasePayload<UpdateWebHookPayload, IUpdateWebHookError>
    {

        /// <summary>
        /// Updated WebHook
        /// </summary>
        public GQL_WebHook hook { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>Handler for <c>UpdateWebHook</c> command </summary>
    public class UpdateWebHookHandler : IRequestHandler<UpdateWebHook, UpdateWebHookPayload>
    {

        /// <summary>
        /// Injected <c>ApiDbContext</c>
        /// </summary>
        private readonly IDbContextFactory<ApiDbContext> _factory;

        /// <summary>
        /// Injected <c>IMediator</c>
        /// </summary>
        private readonly ICurrentUser _current;

        /// <summary>
        /// Injected <c>IMapper</c>
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Main constructor
        /// </summary>
        public UpdateWebHookHandler(
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser currentuser,
            IMapper mapper)
        {
            _mapper = mapper;

            _factory = factory;

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>UpdateWebHook</c>
        /// </summary>
        public async Task<UpdateWebHookPayload> Handle(
            UpdateWebHook request,
            CancellationToken cancellationToken)
        {

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            WebHook wh = await dbContext.WebHooks
            .TagWith(string.Format("UpdateWebHook Command - Query Hook"))
            .Where(e => e.ID == request.WebHookId)
            .FirstOrDefaultAsync(cancellationToken);

            if (wh == null)
            {
                return UpdateWebHookPayload.Error(new WebHookNotFound());
            }

            wh.WebHookUrl = request.WebHookUrl;

            if (request.Secret != null)
            {
                wh.Secret = request.Secret;
            }

            wh.Secret = request.Secret;
            wh.IsActive = request.IsActive;
            wh.HookEvents = request.HookEvents != null ?
                request.HookEvents.Distinct().ToArray() : new HookEventType[0];

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = UpdateWebHookPayload.Success();

            response.hook = _mapper.Map<GQL_WebHook>(wh);

            return response;
        }
    }

    //---------------------------------------
    //---------------------------------------

    public class UpdateWebHookPostProcessor : IRequestPostProcessor<UpdateWebHook, UpdateWebHookPayload>
    {
        /// <summary>
        /// Injected <c>IPublisher</c>
        /// </summary>
        private readonly SharedCore.Aplication.Interfaces.IPublisher _publisher;

        public UpdateWebHookPostProcessor(SharedCore.Aplication.Interfaces.IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Process(
            UpdateWebHook request,
            UpdateWebHookPayload response,
            CancellationToken cancellationToken)
        {
            if (response != null && !response.HasError())
            {
                try
                {

                    // You can extend and add any custom fields to Notification!

                    await _publisher.Publish(new WebHookUpdatedNotifi()
                    {
                        ActivityId = Activity.Current.Id
                    }, PublishStrategy.ParallelNoWait, default(CancellationToken));

                }
                catch { }
            }
        }
    }
}