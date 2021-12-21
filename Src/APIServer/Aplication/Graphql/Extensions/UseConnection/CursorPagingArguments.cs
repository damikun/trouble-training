using HotChocolate.Resolvers;
using HotChocolate.Types.Pagination;

public static class Extensions
{

#nullable enable
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

        var first = context.ArgumentValue<int?>(CursorPagingArgumentNames.First);

        var last = AllowBackwardPagination
            ? context.ArgumentValue<int?>(CursorPagingArgumentNames.Last)
            : null;

        if (first is null && last is null)
        {
            first = DefaultPageSize;
        }

        return new CursorPagingArguments(
            first,
            last,
            context.ArgumentValue<string?>(CursorPagingArgumentNames.After),
            AllowBackwardPagination
                ? context.ArgumentValue<string?>(CursorPagingArgumentNames.Before)
                : null);

    }
#nullable disable

    internal static class CursorPagingArgumentNames
    {
        public const string First = "first";
        public const string After = "after";
        public const string Last = "last";
        public const string Before = "before";
    }
}