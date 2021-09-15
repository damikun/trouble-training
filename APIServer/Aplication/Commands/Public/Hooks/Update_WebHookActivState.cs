using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Aplication.Payload;
using System.Linq;
using FluentValidation;
using Aplication.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using APIServer.Persistence;
using APIServer.Domain.Core.Models.WebHooks;
using Shared.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;

namespace APIServer.Aplication.Commands.WebHooks {

    /// <summary>
    /// Command for updating webhook
    /// </summary>
    [Authorize]
    public class UpdateWebHookActivState : IRequest<UpdateWebHookActivStatePayload> {

        /// <summary>WebHook Id </summary>
        public long WebHookId { get; set; }

        /// <summary> IsActive </summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// UpdateWebHookActivState Validator
    /// </summary>
    public class UpdateWebHookActivStateValidator : AbstractValidator<UpdateWebHookActivState> {

        private readonly IDbContextFactory<ApiDbContext> _factory;

        public UpdateWebHookActivStateValidator(IDbContextFactory<ApiDbContext> factory){
            _factory = factory;
        }

        public UpdateWebHookActivStateValidator() {

            RuleFor(e => e.WebHookId)
            .NotNull()
            .GreaterThan(0);

            RuleFor(e => e.WebHookId)
            .MustAsync(HookExist)
            .WithMessage("Hook was not found");
        }

        public async Task<bool> HookExist(UpdateWebHookActivState request, long id, CancellationToken cancellationToken) {
    
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();
            
            return await dbContext.WebHooks.AnyAsync(e => e.ID == request.WebHookId);
        }
    }

    /// <summary>
    /// IUpdateWebHookActivStateError
    /// </summary>
    public interface IUpdateWebHookActivStateError { }

    /// <summary>
    /// UpdateWebHookActivStatePayload
    /// </summary>
    public class UpdateWebHookActivStatePayload : BasePayload<UpdateWebHookActivStatePayload, IUpdateWebHookActivStateError> {

        /// <summary>
        /// Updated WebHook
        /// </summary>
        public WebHook hook { get; set; }
    }

    /// <summary>Handler for <c>UpdateWebHookActivState</c> command </summary>
    public class UpdateWebHookActivStateHandler : IRequestHandler<UpdateWebHookActivState, UpdateWebHookActivStatePayload> {

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
        public UpdateWebHookActivStateHandler(
            IDbContextFactory<ApiDbContext> factory,
            IMediator mediator,
            ICurrentUser currentuser) {

            _factory = factory;

            _mediator = mediator;

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>UpdateWebHookActivState</c>
        /// </summary>
        public async Task<UpdateWebHookActivStatePayload> Handle(UpdateWebHookActivState request, CancellationToken cancellationToken) {

            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

            WebHook wh = await dbContext.WebHooks
            .TagWith(string.Format("UpdateWebHookActivState Command - Query Hook"))
            .Where(e => e.ID == request.WebHookId)
            .FirstOrDefaultAsync(cancellationToken);

            if (wh == null) {
                return UpdateWebHookActivStatePayload.Error(new WebHookNotFound());
            }

            wh.IsActive = request.IsActive;

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = UpdateWebHookActivStatePayload.Success();

            response.hook = wh;

            return response;

        }
    }
}