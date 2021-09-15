using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Aplication.Payload;
using System.Linq;
using FluentValidation;
using Aplication.Shared.Attributes;
using Microsoft.EntityFrameworkCore;
using APIServer.Aplication.Shared;
using APIServer.Persistence;
using APIServer.Domain.Core.Models.WebHooks;
using Shared.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;

namespace APIServer.Aplication.Commands.WebHooks {

    /// <summary>
    /// Command for updating webhook Uri
    /// </summary>
    [Authorize]
    public class UpdateWebHookUri : IRequest<UpdateWebHookUriPayload> {

        /// <summary>WebHook Id </summary>
        public long WebHookId { get; set; }

        /// <summary> Url </summary>
        public string WebHookUrl { get; set; }
    }

    /// <summary>
    /// UpdateWebHookUri Validator
    /// </summary>
    public class UpdateWebHookUriValidator : AbstractValidator<UpdateWebHookUri> {

        private readonly IDbContextFactory<ApiDbContext> _factory;

        public UpdateWebHookUriValidator(IDbContextFactory<ApiDbContext> factory){
            _factory = factory;
        }

        public UpdateWebHookUriValidator() {

            RuleFor(e => e.WebHookId)
            .NotNull()
            .GreaterThan(0);

            RuleFor(e => e.WebHookUrl)
            .NotEmpty()
            .NotNull();

            RuleFor(e => e.WebHookUrl)
            .Matches(Common.URI_REGEX)
            .WithMessage("Does not match URI expression");

            RuleFor(e => e.WebHookUrl)
            .MaximumLength(1000);

            RuleFor(e => e.WebHookUrl)
            .MustAsync(BeUniqueByURL)
            .WithMessage("Hook endpoint allready exist");

            RuleFor(e => e.WebHookId)
            .MustAsync(HookExist)
            .WithMessage("Hook was not found");
        }

        public async Task<bool> HookExist(UpdateWebHookUri request, long id, CancellationToken cancellationToken) {
    
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();
            
            return await dbContext.WebHooks.AnyAsync(e => e.ID == request.WebHookId);
        }

        public async Task<bool> BeUniqueByURL(UpdateWebHookUri request, string title, CancellationToken cancellationToken) {
            
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();
            
            return await dbContext.WebHooks.AnyAsync(e => e.WebHookUrl == request.WebHookUrl);
        }
    }

    /// <summary>
    /// IUpdateWebHookUriError
    /// </summary>
    public interface IUpdateWebHookUriError { }

    /// <summary>
    /// UpdateWebHookUriPayload
    /// </summary>
    public class UpdateWebHookUriPayload : BasePayload<UpdateWebHookUriPayload, IUpdateWebHookUriError> {

        /// <summary>
        /// Updated WebHook
        /// </summary>
        public WebHook hook { get; set; }
    }

    /// <summary>Handler for <c>UpdateWebHookUri</c> command </summary>
    public class UpdateWebHookUriHandler : IRequestHandler<UpdateWebHookUri, UpdateWebHookUriPayload> {

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
        public UpdateWebHookUriHandler(
            IDbContextFactory<ApiDbContext> factory,
            IMediator mediator,
            ICurrentUser currentuser) {

            _factory = factory;

            _mediator = mediator;

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>UpdateWebHookUri</c>
        /// </summary>
        public async Task<UpdateWebHookUriPayload> Handle(UpdateWebHookUri request, CancellationToken cancellationToken) {
            
            await using ApiDbContext dbContext = 
                _factory.CreateDbContext();

            WebHook wh = await dbContext.WebHooks
            .TagWith(string.Format("UpdateWebHookUri Command - Query Hook"))
            .Where(e => e.ID == request.WebHookId)
            .FirstOrDefaultAsync(cancellationToken);

            if (wh == null) {
                return UpdateWebHookUriPayload.Error(new WebHookNotFound());
            }

            wh.WebHookUrl = request.WebHookUrl;

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = UpdateWebHookUriPayload.Success();

            response.hook = wh;

            return response;

        }
    }
}