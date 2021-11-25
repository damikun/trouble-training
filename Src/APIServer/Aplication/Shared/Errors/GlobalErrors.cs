using APIServer.Aplication.Commands.WebHooks;
using SharedCore.Aplication.GraphQL.Errors;

namespace APIServer.Aplication.Shared.Errors
{

    public interface ITestError { }

    public class UnAuthorised : BaseError, ICreateWebHookError, IRemoveWebHookError, IUpdateWebHookError,
    IUpdateWebHookActivStateError, IUpdateWebHookSecretError, IUpdateWebHookTriggerEventsError,
    IUpdateWebHookUriError, ITestError
    {
        public UnAuthorised()
        {
            this.message = "Unauthorised to process or access resource";
        }

        public UnAuthorised(string s)
        {

            this.message = s;
        }

        public UnAuthorised(object content, string message)
        {

            this.message = message;
        }
    }

    public class InternalServerError : BaseError, ICreateWebHookError, IRemoveWebHookError, IUpdateWebHookError,
    IUpdateWebHookActivStateError, IUpdateWebHookSecretError, IUpdateWebHookTriggerEventsError,
    IUpdateWebHookUriError, ITestError
    {

        public InternalServerError()
        {
            this.message = "Internal server error";
        }

        public InternalServerError(string s)
        {
            this.message = s;
        }
    }

    public class ValidationError : BaseError, ICreateWebHookError, IRemoveWebHookError, IUpdateWebHookError,
    IUpdateWebHookActivStateError, IUpdateWebHookSecretError, IUpdateWebHookTriggerEventsError,
    IUpdateWebHookUriError, ITestError
    {
        public ValidationError()
        {
            this.message = "Some parameter/s are invalid or null";
        }

        public ValidationError(string s)
        {
            this.message = s;
        }

        public ValidationError(string propName, string message)
        {
            this.message = message;
            this.FieldName = propName;
        }

#nullable enable
        public string? FieldName { get; set; }
#nullable disable

    }

    public class BadRequest : BaseError
    {
        public BadRequest()
        {
            this.message = "Bad request";
        }

        public BadRequest(string s)
        {
            this.message = s;
        }
    }

    public class UserDeactivated : BaseError
    {
        public UserDeactivated()
        {
            this.message = "User deactivated";
        }
    }

    public class WebHookNotFound : BaseError, IUpdateWebHookError, IUpdateWebHookActivStateError, IUpdateWebHookUriError,
     IUpdateWebHookSecretError, IRemoveWebHookError, IUpdateWebHookTriggerEventsError
    {
        public WebHookNotFound()
        {
            this.message = "WebHook was not found";
        }

        public WebHookNotFound(string s)
        {
            this.message = s;
        }
    }
}

