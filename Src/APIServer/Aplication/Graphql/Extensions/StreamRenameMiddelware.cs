using System;
using System.Threading.Tasks;
using HotChocolate.Execution;
using System.Linq;
using HotChocolate.Language;
using System.Collections.Generic;
using System.Reflection;

namespace APIServer.Aplication.GraphQL.Extensions
{

    public sealed class StreamArgumentRewriteMiddelware
    {
        private readonly RequestDelegate _next;

        public StreamArgumentRewriteMiddelware(
            RequestDelegate next)
        {
            _next = next ??
                throw new ArgumentNullException(nameof(next));
        }

        public async ValueTask InvokeAsync(IRequestContext context)
        {

            if (context?.Document != null)
            {
                VisitNodes(context.Document.GetNodes());

                await _next(context).ConfigureAwait(false);
            }

        }

        public static void VisitNodes(IEnumerable<ISyntaxNode> nodes)
        {

            if (nodes == null)
            {
                return;
            }

            foreach (var item in nodes)
            {
                if (item != null && item is ISelectionNode selectionNode)
                {

                    foreach (var directive in selectionNode?.Directives?.Where(s => s?.Name?.Value == "stream"))
                    {
                        if (directive?.Arguments != null)
                        {
                            RenameArgument(directive.Arguments
                                .Where(e => e.Name?.Value == "initial_count")
                            );
                        }
                    }
                }

                if (item != null)
                {
                    VisitNodes(item.GetNodes());
                }
            }
        }

        public static void RenameArgument(IEnumerable<ArgumentNode> arguments)
        {
            foreach (var argument in arguments)
            {
                if (argument != null)
                {
                    argument.Name.WithValue("initialCount");

                    var some = typeof(NameNode).GetField(
                        "<Value>k__BackingField",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );

                    some?.SetValue(argument.Name, "initialCount");
                }
            }
        }

    }
}