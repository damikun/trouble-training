using System;
using HotChocolate;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types.Pagination;
using HotChocolate.Types.Descriptors;
using System.Runtime.CompilerServices;
using HotChocolate.Types.Descriptors.Definitions;

namespace APIServer.Aplication.GraphQL.Extensions
{
    public sealed class UseConnection : ObjectFieldDescriptorAttribute
    {

#nullable enable
        private string? _connectionName;
        private int? _defaultPageSize;
        private int? _maxPageSize;
        private bool? _includeTotalCount;
        private bool? _allowBackwardPagination;
        private bool? _requirePagingBoundaries;
        private bool? _inferConnectionNameFromField;
#nullable disable

#nullable enable
        public UseConnection(Type? type = null, [CallerLineNumber] int order = 0)
        {
            Type = type;
            Order = order;
        }
#nullable disable

        /// <summary>
        /// The schema type representation of the node type.
        /// </summary>
#nullable enable
        public Type? Type { get; private set; }
#nullable disable


        /// <summary>
        /// Specifies the connection name.
        /// </summary>
#nullable enable
        public string? ConnectionName
        {
            get => _connectionName;
            set => _connectionName = value;
        }
#nullable disable

        /// <summary>
        /// Specifies the default page size for this field.
        /// </summary>
        public int DefaultPageSize
        {
            get => _defaultPageSize ?? PagingDefaults.DefaultPageSize;
            set => _defaultPageSize = value;
        }

        /// <summary>
        /// Specifies the maximum allowed page size.
        /// </summary>
        public int MaxPageSize
        {
            get => _maxPageSize ?? PagingDefaults.MaxPageSize;
            set => _maxPageSize = value;
        }

        /// <summary>
        /// Include the total count field to the result type.
        /// </summary>
        public bool IncludeTotalCount
        {
            get => _includeTotalCount ?? PagingDefaults.IncludeTotalCount;
            set => _includeTotalCount = value;
        }

        /// <summary>
        /// Allow backward paging using <c>last</c> and <c>before</c>
        /// </summary>
        public bool AllowBackwardPagination
        {
            get => _allowBackwardPagination ?? PagingDefaults.AllowBackwardPagination;
            set => _allowBackwardPagination = value;
        }

        /// <summary>
        /// Defines if the paging middleware shall require the
        /// API consumer to specify paging boundaries.
        /// </summary>
        public bool RequirePagingBoundaries
        {
            get => _requirePagingBoundaries ?? PagingDefaults.AllowBackwardPagination;
            set => _requirePagingBoundaries = value;
        }

        /// <summary>
        /// Connection names are by default inferred from the field name to
        /// which they are bound to as opposed to the node type name.
        /// </summary>
        public bool InferConnectionNameFromField
        {
            get => _inferConnectionNameFromField ?? PagingDefaults.AllowBackwardPagination;
            set => _inferConnectionNameFromField = value;
        }

        /// <summary>
        /// Specifies the name of the paging provider that shall be used.
        /// </summary>
#nullable enable
        public string? ProviderName { get; set; }

#nullable disable

        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {

            if (descriptor is null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            descriptor.AddPagingArguments(true);

            var options = new PagingOptions();

            descriptor
                .Extend()
                .OnBeforeCreate((c, d) =>
                {
                    PagingOptions pagingOptions = c.GetSettings(options);
                    var backward = pagingOptions.AllowBackwardPagination ?? AllowBackwardPagination;

                    ObjectFieldDefinition definition = descriptor.Extend().Definition;

                    var connectionName =
                        pagingOptions.InferConnectionNameFromField ??
                        InferConnectionNameFromField
                            ? (NameString?)EnsureConnectionNameCasing(d.Name)
                                : null;
#nullable enable
                    ITypeReference? typeRef = Type is not null
                        ? c.TypeInspector.GetTypeRef(Type)
                        : null;
#nullable disable
                    if (typeRef is null &&
                        d.Type is SyntaxTypeReference syntaxTypeRef &&
                        syntaxTypeRef.Type.IsListType())
                    {
                        typeRef = syntaxTypeRef.WithType(syntaxTypeRef.Type.ElementType());
                    }

#nullable enable
                    MemberInfo? resolverMember = d.ResolverMember ?? d.Member;
#nullable disable
                    string methodName = "CreateConnectionTypeRef";

                    Type type = typeof(PagingObjectFieldDescriptorExtensions);

                    MethodInfo info = type.GetMethod(
                        methodName,
                        BindingFlags.NonPublic |
                        BindingFlags.Public |
                        BindingFlags.Static |
                        BindingFlags.FlattenHierarchy
                    );

#nullable enable
                    d.Type = (ITypeReference?)info?.Invoke(null, new object[] {
                         c,
                         resolverMember!,
                         connectionName!,
                         typeRef!,
                         options
                    });
#nullable disable

                    d.CustomSettings.Add(typeof(Connection));
                });
        }

        private static NameString EnsureConnectionNameCasing(string connectionName)
        {
            if (char.IsUpper(connectionName[0]))
            {
                return connectionName;
            }

            return string.Concat(char.ToUpper(
                connectionName[0]),
                connectionName.Substring(1)
            );
        }
    }

    //---------------------------------------
    //---------------------------------------

    public static class Extensions
    {

        public static CursorPagingArguments GetPaggingArguments(
            this IResolverContext context,
            bool AllowBackwardPagination = true,
            int DefaultPageSize = 10)
        {
            var MaxPageSize = int.MaxValue;

            if (MaxPageSize < DefaultPageSize)
            {
                DefaultPageSize = MaxPageSize;
            }
#nullable enable
            var first = context.ArgumentValue<int?>(CursorPagingArgumentNames.First);

            var last = AllowBackwardPagination
                ? context.ArgumentValue<int?>(CursorPagingArgumentNames.Last)
                : null;
#nullable disable
            if (first is null && last is null)
            {
                first = DefaultPageSize;
            }
#nullable enable
            return new CursorPagingArguments(
                first,
                last,
                context.ArgumentValue<string?>(CursorPagingArgumentNames.After),
                AllowBackwardPagination
                    ? context.ArgumentValue<string?>(CursorPagingArgumentNames.Before)
                    : null);
#nullable disable
        }

        internal static class CursorPagingArgumentNames
        {
            public const string First = "first";
            public const string After = "after";
            public const string Last = "last";
            public const string Before = "before";
        }
    }
}