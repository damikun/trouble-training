using System;
using FluentValidation.Results;

namespace APIServer.Aplication.Shared.Exceptions {

    public class AuthorizationException : Exception {
        public AuthorizationException()
            : base("One or more authorization failures have occurred.") {
            Errors = new ValidationFailure[0];
        }

        public AuthorizationException(ValidationFailure[] failures)
            : this() {
            Errors = failures;
        }

        public AuthorizationException(string message)
        : base(message) {
            Errors = new ValidationFailure[0];
        }

        public ValidationFailure[] Errors { get; }
    }

}