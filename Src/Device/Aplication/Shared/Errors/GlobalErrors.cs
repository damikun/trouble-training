using Device.Aplication.Commands.Test;
using SharedCore.Aplication.GraphQL.Errors;

namespace Device.Aplication.Shared.Errors {

    public class UnAuthorised : BaseError, ITrigger_AuthorisedError, ITrigger_UnAuthorisedError {
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

    public class InternalServerError : BaseError, ITrigger_AuthorisedError, ITrigger_UnAuthorisedError {

        public InternalServerError() {
            this.message = "Internal server error";
        }

        public InternalServerError(string s) {
            this.message = s;
        }
    }

    public class ValidationError : BaseError, ITrigger_AuthorisedError, ITrigger_UnAuthorisedError {
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
        #nullable enable
        public string? FieldName { get; set; }
        #nullable disable
    }

}