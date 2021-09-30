using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using FluentValidation;
using SharedCore.Aplication.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using APIServer.Persistence;
using APIServer.Domain.Core.Models.WebHooks;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;
using Aplication.Payload;

namespace APIServer.Aplication.Commands.WebHooks {

    /// <summary>
    /// Command for updateing WebHook secret
    /// </summary>
    [Authorize]
    public class UpdateWebHookSecret : IRequest<UpdateWebHookSecretPayload> {

        /// <summary>WebHook Id </summary>
        public long WebHookId { get; set; }

        /// <summary> Secret </summary>
        public string Secret { get; set; }
    }

    /// <summary>
    /// UpdateWebHookSecret Validator
    /// </summary>
    public class UpdateWebHookSecretValidator : AbstractValidator<UpdateWebHookSecret> {

        private readonly IDbContextFactory<ApiDbContext> _factory;

        public UpdateWebHookSecretValidator(IDbContextFactory<ApiDbContext> factory){
            _factory = factory;
            
            RuleFor(e => e.WebHookId)
            .NotNull()
            .GreaterThan(0);

            RuleFor(e => e.Secret)
            .MaximumLength(1000);

            RuleFor(e => e.WebHookId)
            .MustAsync(HookExist)
            .WithMessage("Hook was not found");
        }

        public async Task<bool> HookExist(UpdateWebHookSecret request, long id, CancellationToken cancellationToken) {
    
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();
            
            return await dbContext.WebHooks.AnyAsync(e => e.ID == request.WebHookId);
        }
    }

    /// <summary>
    /// IUpdateWebHookSecretError
    /// </summary>
    public interface IUpdateWebHookSecretError { }

    /// <summary>
    /// UpdateWebHookSecretPayload
    /// </summary>
    public class UpdateWebHookSecretPayload : BasePayload<UpdateWebHookSecretPayload, IUpdateWebHookSecretError> {

        /// <summary>
        /// Updated WebHook
        /// </summary>
        public WebHook hook { get; set; }
    }

    /// <summary>Handler for <c>UpdateWebHookSecret</c> command </summary>
    public class UpdateWebHookSecretHandler : IRequestHandler<UpdateWebHookSecret, UpdateWebHookSecretPayload> {

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
        public UpdateWebHookSecretHandler(
            IDbContextFactory<ApiDbContext> factory,
            IMediator mediator,
            ICurrentUser currentuser) {

            _factory = factory;

            _mediator = mediator;

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>UpdateWebHookSecret</c>
        /// </summary>
        public async Task<UpdateWebHookSecretPayload> Handle(UpdateWebHookSecret request, CancellationToken cancellationToken) {

            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

            WebHook wh = await dbContext.WebHooks
            .TagWith(string.Format("UpdateWebHookSecret Command - Query Hook"))
            .Where(e => e.ID == request.WebHookId)
            .FirstOrDefaultAsync(cancellationToken);

            if (wh == null) {
                return UpdateWebHookSecretPayload.Error(new WebHookNotFound());
            }

            wh.Secret = request.Secret;

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = UpdateWebHookSecretPayload.Success();

            response.hook = wh;

            return response;
        }
    }
}