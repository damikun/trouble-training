using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using APIServer.Aplication.GraphQL.DTO;
using SharedCore.Aplication.Interfaces;

namespace APIServer.Aplication.GraphQL.Queries {
    
    /// <summary>
    /// UserQueries
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserQueries {
            public async Task<GQL_User> me(
            [Service] ICurrentUser _current,
            [Service] IHttpContextAccessor httpcontext) {

            if (!_current.Exist) {
                return null;
            }

            await Task.CompletedTask;

            return new GQL_User(){
                Guid = _current.UserId !=null?_current.UserId.Value.ToString():"",
                Name = _current.GetClaim("name"),
                Email = _current.GetClaim("email"),
                SessionId = _current.GetClaim("sid"),
            };
        }
    }
}