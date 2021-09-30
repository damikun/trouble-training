using Device.Aplication.Interfaces;

namespace Device.Aplication.Shared.Errors {

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

    public class InternalServerError : BaseError {

        public InternalServerError() {
            this.message = "Internal server error";
        }

        public InternalServerError(string s) {
            this.message = s;
        }
    }

    public class ValidationError : BaseError {
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