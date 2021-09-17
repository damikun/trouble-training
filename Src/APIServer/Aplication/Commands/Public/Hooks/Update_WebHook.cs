using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Aplication.Payload;
using System.Linq;
using FluentValidation;
using Aplication.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Aplication.Shared;
using APIServer.Persistence;
using Shared.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;

namespace APIServer.Aplication.Commands.WebHooks {

    /// <summary>
    /// Command for updating webhook
    /// </summary>
    [Authorize]
    public class UpdateWebHook : IRequest<UpdateWebHookPayload> {

        public UpdateWebHook() {
            this.HookEvents = new HashSet<HookEventType>();
        }

        /// <summary>WebHook Id </summary>
        public long WebHookId { get; set; }

        /// <summary> Url </summary>
        public string WebHookUrl { get; set; }

        /// <summary> Secret </summary>
        public string? Secret { get; set; }

        /// <summary> IsActive </summary>
        public bool IsActive { get; set; }

        /// <summary> HookEvents </summary>
        public HashSet<HookEventType> HookEvents { get; set; }
    }

    /// <summary>
    /// UpdateWebHook Validator
    /// </summary>
    public class UpdateWebHookValidator : AbstractValidator<UpdateWebHook> {

        private readonly IDbContextFactory<ApiDbContext> _factory;

        public UpdateWebHookValidator(IDbContextFactory<ApiDbContext> factory){
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
            .MustAsync( async (command,url,cancellation) =>  await BeUniqueByURL(url,command.WebHookId, cancellation)
            ).WithMessage("Hook endpoint allready exist");

            RuleFor(e => e.WebHookId)
            .MustAsync(HookExist)
            .WithMessage("Hook was not found");
        }

        public async Task<bool>  BeUniqueByURL(string url,long hook_id, CancellationToken cancellationToken) {
            
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

            return !await dbContext.WebHooks.AnyAsync(e => e.WebHookUrl == url && e.ID != hook_id);
        }

        public async Task<bool> HookExist(long hook_id, CancellationToken cancellationToken) {
            
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();
            
            return (await dbContext.WebHooks.AnyAsync(e=>e.ID == hook_id));
        }
    }

    /// <summary>
    /// IUpdateWebHookError
    /// </summary>
    public interface IUpdateWebHookError { }

    /// <summary>
    /// UpdateWebHookPayload
    /// </summary>
    public class UpdateWebHookPayload : BasePayload<UpdateWebHookPayload, IUpdateWebHookError> {

        /// <summary>
        /// Updated WebHook
        /// </summary>
        public WebHook hook { get; set; }
    }

    /// <summary>Handler for <c>UpdateWebHook</c> command </summary>
    public class UpdateWebHookHandler : IRequestHandler<UpdateWebHook, UpdateWebHookPayload> {

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
        public UpdateWebHookHandler(
            IDbContextFactory<ApiDbContext> factory,
            IMediator mediator,
            ICurrentUser currentuser) {

            _factory = factory;

            _mediator = mediator;

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>UpdateUserGroup</c>
        /// </summary>
        public async Task<UpdateWebHookPayload> Handle(UpdateWebHook request, CancellationToken cancellationToken) {

            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

            WebHook wh = await dbContext.WebHooks
            .TagWith(string.Format("UpdateWebHook Command - Query Hook"))
            .Where(e => e.ID == request.WebHookId)
            .FirstOrDefaultAsync(cancellationToken);

            if (wh == null) {
                return UpdateWebHookPayload.Error(new WebHookNotFound());
            }

            wh.WebHookUrl = request.WebHookUrl;

            if (request.Secret != null) {
                wh.Secret = request.Secret;
            }

            wh.Secret = request.Secret;
            wh.IsActive = request.IsActive;
            wh.HookEvents = request.HookEvents != null ? request.HookEvents.Distinct().ToArray() : new HookEventType[0];

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = UpdateWebHookPayload.Success();

            response.hook = wh;

            return response;
        }
    }
}