
using SharedCore.Aplication.GraphQL.Interfaces;

namespace SharedCore.Aplication.GraphQL.Errors {
    public class BaseError : IBaseError {
            public string message { get; set; }
    }
}