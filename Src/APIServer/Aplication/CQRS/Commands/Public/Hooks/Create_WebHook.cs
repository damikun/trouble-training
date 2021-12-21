using MediatR;
using AutoMapper;
using System.Linq;
using System.Threading;
using FluentValidation;
using MediatR.Pipeline;
using System.Diagnostics;
using APIServer.Persistence;
using System.Threading.Tasks;
using System.Collections.Generic;
using APIServer.Aplication.Shared;
using SharedCore.Aplication.Payload;
using Microsoft.EntityFrameworkCore;
using SharedCore.Aplication.Services;
using APIServer.Aplication.GraphQL.DTO;
using SharedCore.Aplication.Interfaces;
using SharedCore.Aplication.Core.Commands;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Aplication.Shared.Behaviours;
using SharedCore.Aplication.Shared.Attributes;
using APIServer.Aplication.Notifications.WebHooks;

namespace APIServer.Aplication.Commands.WebHooks
{
    /// <summary>
    /// Command for creating webhook
    /// </summary>
    [Authorize] // <-- Activate Auth check for command
    // [Authorize(FieldPolicy = true)] <-- Uncommend to activate Field Auth check for command
    public class CreateWebHook : CommandBase<CreateWebHookPayload>
    {

        public CreateWebHook()
        {
            this.HookEvents = new HashSet<HookEventType>();
        }

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
    /// CreateWebHook Validator
    /// </summary>
    public class CreateWebHookValidator : AbstractValidator<CreateWebHook>
    {
        private readonly IDbContextFactory<ApiDbContext> _factory;

        const long MAX_HOOK_COUNT = 10;

        public CreateWebHookValidator(IDbContextFactory<ApiDbContext> factory)
        {
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

        public async Task<bool> BeUniqueByURL(
            string url,
            CancellationToken cancellationToken)
        {

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            return await dbContext.WebHooks.AllAsync(e => e.WebHookUrl != url);
        }

        public async Task<bool> CheckMaxAllowedHooksCount(
            string url,
            CancellationToken cancellationToken)
        {

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            return (await dbContext.WebHooks.CountAsync()) <= MAX_HOOK_COUNT;
        }
    }

    /// <summary>
    /// Authorization validators for CreateWebHook
    /// </summary>
    public class CreateWebHookAuthorizationValidator : AuthorizationValidator<CreateWebHook>
    {

        private readonly IDbContextFactory<ApiDbContext> _factory;
        public CreateWebHookAuthorizationValidator(IDbContextFactory<ApiDbContext> factory)
        {

            _factory = factory;

            // Add Field authorization cehcks.. (use  [Authorize(FieldPolicy = true)] to activate)
        }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>
    /// ICreateWebHookError
    /// </summary>
    public interface ICreateWebHookError { }

    /// <summary>
    /// CreateWebHookPayload
    /// </summary>
    public class CreateWebHookPayload : BasePayload<CreateWebHookPayload, ICreateWebHookError>
    {

        /// <summary>
        /// Created WebHook
        /// </summary>
        public GQL_WebHook hook { get; set; }
    }

    //---------------------------------------
    //---------------------------------------

    /// <summary>Handler for <c>CreateWebHook</c> command </summary>
    public class CreateWebHookHandler : IRequestHandler<CreateWebHook, CreateWebHookPayload>
    {

        /// <summary>
        /// Injected IDbContextFactory of ApiDbContext
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

        /// <summary>
        /// Main constructor
        /// </summary>
        public CreateWebHookHandler(
            IDbContextFactory<ApiDbContext> factory,
            ICurrentUser currentuser,
            IMapper mapper)
        {
            _mapper = mapper;

            _factory = factory;

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>CreateWebHook</c>
        /// </summary>
        public async Task<CreateWebHookPayload> Handle(
            CreateWebHook request, CancellationToken cancellationToken)
        {

            await using ApiDbContext dbContext =
                _factory.CreateDbContext();

            WebHook hook = new WebHook
            {
                WebHookUrl = request.WebHookUrl,
                Secret = request.Secret,
                ContentType = "application/json",
                IsActive = request.IsActive,
                HookEvents = request.HookEvents != null ?
                    request.HookEvents.Distinct().ToArray() : new HookEventType[0]
            };

            dbContext.WebHooks.Add(hook);

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = CreateWebHookPayload.Success();

            response.hook = _mapper.Map<GQL_WebHook>(hook);

            return response;
        }
    }

    //---------------------------------------
    //---------------------------------------

    public class CreateWebHookPostProcessor
        : IRequestPostProcessor<CreateWebHook, CreateWebHookPayload>
    {
        /// <summary>
        /// Injected <c>IPublisher</c>
        /// </summary>
        private readonly SharedCore.Aplication.Interfaces.IPublisher _publisher;

        public CreateWebHookPostProcessor(SharedCore.Aplication.Interfaces.IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task Process(
            CreateWebHook request,
            CreateWebHookPayload response,
            CancellationToken cancellationToken)
        {
            if (response != null && !response.HasError())
            {
                try
                {
                    // You can extend and add any custom fields to Notification!
                    await _publisher.Publish(new WebHookCreatedNotifi()
                    {
                        ActivityId = Activity.Current.Id
                    }, PublishStrategy.ParallelNoWait, default(CancellationToken));

                }
                catch { }
            }
        }
    }
}