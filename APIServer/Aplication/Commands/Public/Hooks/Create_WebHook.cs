using MediatR;
using System.Linq;
using System.Threading;
using FluentValidation;
using Aplication.Payload;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.Aplication.Interfaces;
using Aplication.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Persistence;
using APIServer.Aplication.Shared;

namespace APIServer.Aplication.Commands.WebHooks {

    /// <summary>
    /// Command for creating webhook
    /// </summary>
    [Authorize]
    public class CreateWebHook : IRequest<CreateWebHookPayload> {

        public CreateWebHook() {
            this.HookEvents = new HashSet<HookEventType>();
        }

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
    /// CreateWebHook Validator
    /// </summary>
    public class CreateWebHookValidator : AbstractValidator<CreateWebHook> {

        private readonly IDbContextFactory<ApiDbContext> _factory;

        public CreateWebHookValidator(IDbContextFactory<ApiDbContext> factory){
            _factory = factory;

            RuleFor(e => e.WebHookUrl)
            .NotEmpty()
            .NotNull();
            
            RuleFor(e => e.WebHookUrl)
            .Matches(Common.URI_REGEX)
            .WithMessage("Does not match URI expression");

            RuleFor(e => e.WebHookUrl)
            .MustAsync(BeUniqueByURL)
            .WithMessage("Hook endpoint allready exist");

            RuleFor(e => e.WebHookUrl)
            .MustAsync(CheckMaxAllowedHooksCount)
            .WithMessage("Max allowed hooks count detected");

            RuleFor(e => e.HookEvents)
            .NotNull();
        }

        public async Task<bool>  BeUniqueByURL(string url, CancellationToken cancellationToken) {
            
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

            return await dbContext.WebHooks.AllAsync(e => e.WebHookUrl != url);
        }

        public async Task<bool> CheckMaxAllowedHooksCount(string url, CancellationToken cancellationToken) {
            
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

            const long  MAX_HOOK_COUNT = 3;
            
            return (await dbContext.WebHooks.CountAsync()) <= MAX_HOOK_COUNT;
        }
    }

    /// <summary>
    /// ICreateWebHookError
    /// </summary>
    public interface ICreateWebHookError { }

    /// <summary>
    /// CreateWebHookPayload
    /// </summary>
    public class CreateWebHookPayload : BasePayload<CreateWebHookPayload, ICreateWebHookError> {

        /// <summary>
        /// Created WebHook
        /// </summary>
        public WebHook hook { get; set; }
    }

    /// <summary>Handler for <c>CreateWebHook</c> command </summary>
    public class CreateWebHookHandler : IRequestHandler<CreateWebHook, CreateWebHookPayload> {

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
        public CreateWebHookHandler(
            IDbContextFactory<ApiDbContext> factory,
            IMediator mediator,
            ICurrentUser currentuser) {

            _factory = factory;

            _mediator = mediator;

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>CreateWebHook</c>
        /// </summary>
        public async Task<CreateWebHookPayload> Handle(CreateWebHook request, CancellationToken cancellationToken) {

            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

                WebHook hook = new WebHook {
                    WebHookUrl = request.WebHookUrl,
                    Secret = request.Secret,
                    ContentType = "application/json",
                    IsActive = request.IsActive,
                    HookEvents = request.HookEvents != null ? request.HookEvents.Distinct().ToArray() : new HookEventType[0]
                };

                dbContext.WebHooks.Add(hook);

                await dbContext.SaveChangesAsync(cancellationToken);

                var response = CreateWebHookPayload.Success();

                response.hook = hook;

                return response;
        }
    }
}