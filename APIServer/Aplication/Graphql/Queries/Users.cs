using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using APIServer.Aplication.GraphQL.DTO;
using Shared.Aplication.Interfaces;

namespace APIServer.Aplication.GraphQL.Queries {
    
        /// <summary>
    /// UserQueries
    /// </summary>
    [ExtendObjectType(Name = "Querry")]
    public class UserQueries {
            public async Task<GQL_User> me(
            [Service] ICurrentUser _current,
            [Service] IHttpContextAccessor httpcontext) {

            if (!_current.Exist) {
                return null;
            }

            return new GQL_User(){
                Guid = _current.UserId !=null?_current.UserId.Value.ToString():"",
                FirstName = _current.Name,
            };
        }
    }
}