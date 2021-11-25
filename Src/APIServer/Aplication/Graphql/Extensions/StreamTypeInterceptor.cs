using System.Linq;
using HotChocolate.Types;
using System.Collections.Generic;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors.Definitions;

namespace APIServer.Aplication.GraphQL.Extensions
{

    // This is important sinde currently Relay does not follow oficial GraphQL specification for @stream argument naming

    public class StreamTypeInterceptor : TypeInterceptor
    {

#nullable enable
        public override void OnBeforeCompleteType(
        ITypeCompletionContext completionContext,
        DefinitionBase? definition,
        IDictionary<string, object?> contextData)
        {
#nullable disable

            if (definition is DirectiveTypeDefinition directiveTypeDefinition
            && directiveTypeDefinition?.RuntimeType == typeof(StreamDirective))
            {

                var InitCountArg = directiveTypeDefinition.Arguments
                    .First(e => e.Property?.Name == "InitialCount");

                if (InitCountArg != null)
                {
                    InitCountArg.Name = "initial_count";
                }
            }
        }
    }
}