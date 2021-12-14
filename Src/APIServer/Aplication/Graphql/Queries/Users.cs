using MediatR;
using HotChocolate;
using HotChocolate.Types;
using System.Threading.Tasks;
using APIServer.Aplication.Queries;
using APIServer.Aplication.GraphQL.DTO;

namespace APIServer.Aplication.GraphQL.Queries
{
    /// <summary>
    /// UserQueries
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserQueries
    {
        public async Task<GQL_User> me([Service] IMediator mediator)
        {
            var response = await mediator.Send(new GetCurrentUser());

            return response.user;
        }
    }
}