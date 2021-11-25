using System.Threading.Tasks;
using SharedCore.Aplication.Payload;
using APIServer.Aplication.Shared.Errors;
using SharedCore.Aplication.Core.Commands;
using SharedCore.Aplication.GraphQL.Errors;
using MediatR;

namespace APIServer.Application.UnitTests.Behaviours
{
    public class ExceptionTestCommand : CommandBase<ExceptionTestPayload> { }

    public class ExceptionTestPayload : BasePayload<ExceptionTestPayload, ITestError>
    {
        public object data { get; set; }
    }

    //---------------------------------

    public interface IExceptionUnknownError { }

    public class ExceptionUnknownCommand
    : CommandBase<ExceptionUnknownCommandPayload>
    { }

    public class ExceptionUnknownCommandPayload
    : BasePayload<ExceptionUnknownCommandPayload, IExceptionUnknownError>
    { }

    //---------------------------------

    public class ExceptionQuery : IRequest<string> { }

    public class ExceptionQueryResponse { }

    //---------------------------------

    public class ExceptionUnsupportedError : BaseError
    {

        public ExceptionUnsupportedError()
        {
            this.message = "Test unknown error";
        }
    }

    //---------------------------------

    public class ExceptionTestCommandHandler<T> where T : IBasePayload, new()
    {

        public ExceptionTestCommandHandler() { }

        public async Task<T> HandleWithThrow()
        {

            await Task.CompletedTask;

            throw new System.Exception();
        }

        public async Task<T> HandleWithUnknownPayloadError()
        {

            await Task.CompletedTask;

            var payload = new T();

            payload.AddError(new ExceptionUnsupportedError());

            return payload;
        }

        public async Task<T> HandleWithoutThrow()
        {

            await Task.CompletedTask;

            return new T();
        }
    }

    //---------------------------------

    public class ExceptionTestQueryHandler<T> where T : new()
    {

        public ExceptionTestQueryHandler() { }

        public async Task<T> HandleWithThrow()
        {

            await Task.CompletedTask;

            throw new System.Exception();
        }

        public async Task<T> HandleWithoutThrow()
        {

            await Task.CompletedTask;

            return new T();
        }
    }

}