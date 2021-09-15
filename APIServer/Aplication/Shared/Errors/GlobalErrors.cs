using APIServer.Aplication.Commands.WebHooks;
using APIServer.Aplication.Interfaces;

namespace APIServer.Aplication.Shared.Errors {

    public class BaseError : IBaseError {
        public string message { get; set; }
    }

    public class UnAuthorised : BaseError {
        public UnAuthorised() {
            this.message = "Unauthorised to process or access resource";
        }

        public UnAuthorised(string s) {

            this.message = s;
        }

        public UnAuthorised(object content, string message) {

            this.message = message;
        }
    }

    public class InternalServerError : BaseError{

        public InternalServerError() {
            this.message = "Internal server error";
        }

        public InternalServerError(string s) {
            this.message = s;
        }
    }

    public class ValidationError : BaseError{
        public ValidationError() {
            this.message = "Some parameter/s are invalid or null";
        }

        public ValidationError(string s) {
            this.message = s;
        }

        public ValidationError(string propName, string message) {
            this.message = message;
            this.FieldName = propName;
        }
        public string? FieldName { get; set; }

    }

    public class BadRequest : BaseError{
        public BadRequest() {
            this.message = "Bad request";
        }

        public BadRequest(string s) {
            this.message = s;
        }
    }

    public class UserDeactivated : BaseError {
        public UserDeactivated() {
            this.message = "User deactivated";
        }
    }

    public class WebHookNotFound : BaseError, IUpdateWebHookError, IUpdateWebHookActivStateError, IUpdateWebHookUriError,
     IUpdateWebHookSecretError, IRemoveWebHookError, IUpdateWebHookTriggerEventsError {
        public WebHookNotFound() {
            this.message = "WebHook was not found";
        }

        public WebHookNotFound(string s) {
            this.message = s;
        }
    }
}

