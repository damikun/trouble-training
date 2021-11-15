
using APIServer.Aplication.GraphQL.Queries;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types
{

    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {

            // Extend query hire using code-first
        }
    }
}
