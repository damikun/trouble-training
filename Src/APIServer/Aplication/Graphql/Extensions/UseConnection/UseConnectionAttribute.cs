using System;
using HotChocolate;
using System.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using HotChocolate.Types.Descriptors;
using System.Runtime.CompilerServices;

namespace APIServer.Aplication.GraphQL.Extensions
{
    public sealed class UseConnectionAttribute : ObjectFieldDescriptorAttribute
    {

#nullable enable
        private string? _connectionName;
        private int? _defaultPageSize;
        private int? _maxPageSize;
        private bool? _includeTotalCount;
        private bool? _allowBackwardPagination;
        private bool? _requirePagingBoundaries;
        private bool? _inferConnectionNameFromField;

        public UseConnectionAttribute(
            Type? type = null,
            [CallerLineNumber] int order = 0)
        {
            Type = type;
            Order = order;
        }

        /// <summary>
        /// The schema type representation of the node type.
        /// </summary>
        public Type? Type { get; private set; }

        /// <summary>
        /// Specifies the connection name.
        /// </summary>
        public string? ConnectionName
        {
            get => _connectionName;
            set => _connectionName = value;
        }

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

            descriptor.UseConnection(
            nodeType: Type,
            connectionName: string.IsNullOrEmpty(_connectionName)
                            ? default(NameString?)
                            : _connectionName,
            options: new PagingOptions
            {
                DefaultPageSize = _defaultPageSize,
                MaxPageSize = _maxPageSize,
                IncludeTotalCount = _includeTotalCount,
                AllowBackwardPagination = _allowBackwardPagination,
                RequirePagingBoundaries = _requirePagingBoundaries,
                InferConnectionNameFromField = _inferConnectionNameFromField,
                ProviderName = ProviderName
            });
        }
    }
}