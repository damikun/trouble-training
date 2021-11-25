using System;
using System.ComponentModel;
using System.Security.Claims;

namespace SharedCore.Aplication.Extensions
{

    public static partial class Extensions
    {
        public static TId GetId<TId>(this ClaimsPrincipal principal)
        {
            if (principal == null || principal.Identity == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            if (!principal.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException(nameof(principal));
            }

            var loggedInUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (typeof(TId) == typeof(string) ||
                typeof(TId) == typeof(int) ||
                typeof(TId) == typeof(long) ||
                typeof(TId) == typeof(Guid))
            {
                var converter = TypeDescriptor.GetConverter(typeof(TId));

                return (TId)converter.ConvertFromInvariantString(loggedInUserId);
            }

            throw new InvalidOperationException("The user id type is invalid");
        }

        public static Guid GetId(this ClaimsPrincipal principal)
        {
            return principal.GetId<Guid>();
        }

    }
}
