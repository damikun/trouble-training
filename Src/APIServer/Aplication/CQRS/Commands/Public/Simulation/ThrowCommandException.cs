using MediatR;
using System.Threading;
using FluentValidation;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIServer.Persistence;
using SharedCore.Aplication.Payload;

namespace APIServer.Aplication.Commands.WebHooks
{

    /// <summary>
    /// Command for testing throwing command exception
    /// </summary>
    public class ThrowCommandException
        : IRequest<ThrowCommandExceptionPayload>
    {

        public ThrowCommandException() { }
    }

    /// <summary>
    /// ThrowCommandException Validator
    /// </summary>
    public class ThrowCommandExceptionValidator
        : AbstractValidator<ThrowCommandException>
    {

        private readonly IDbContextFactory<ApiDbContext> _factory;

        public ThrowCommandExceptionValidator(
            IDbContextFactory<ApiDbContext> factory)
        {
            _factory = factory;
        }

    }

    /// <summary>
    /// IThrowCommandExceptionError
    /// </summary>
    public interface IThrowCommandExceptionError { }

    /// <summary>
    /// ThrowCommandExceptionPayload
    /// </summary>
    public class ThrowCommandExceptionPayload
        : BasePayload<ThrowCommandExceptionPayload, IThrowCommandExceptionError>
    {
    }

    /// <summary>Handler for <c>ThrowCommandException</c> command </summary>
    public class ThrowCommandExceptionHandler
        : IRequestHandler<ThrowCommandException, ThrowCommandExceptionPayload>
    {

        private static bool IsEnabled = true;

        /// <summary>
        /// Main constructor
        /// </summary>
        public ThrowCommandExceptionHandler()
        {

        }

        /// <summary>
        /// Command handler for <c>ThrowCommandException</c>
        /// </summary>
        public async Task<ThrowCommandExceptionPayload> Handle(
            ThrowCommandException request,
            CancellationToken cancellationToken)
        {

            if (IsEnabled)
            {
                throw new System.Exception(
                    "This is simulation of command problem");
            }

            await Task.CompletedTask;

            return ThrowCommandExceptionPayload.Success();
        }
    }
}