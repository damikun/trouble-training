using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Aplication.Interfaces;
using Shared.Aplication.Extensions;
using System;

namespace Shared.Aplication.Services {

    /// <summary>DI object of current user ID Resolver</summary>
    public class CurrentUser : ICurrentUser {

        /// <summary>DI object of ILogger</summary>
        private readonly ILogger<CurrentUser> _logger;

        /// <summary>DI object of IHttpContextAccessor</summary>
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Main constructor of CurrentUserProvider
        /// </summary>
        public CurrentUser(
            ILogger<CurrentUser> logger,
            IHttpContextAccessor contextAccessor
        ) {
            _logger = logger;

            _contextAccessor = contextAccessor;
        }

        public bool Exist {
            get {
                return UserId != null ? true: false;
            }
        }

        public Guid? UserId {

            get {

                foreach (var item in _contextAccessor.HttpContext.User.Claims)
                {
                     System.Console.WriteLine(item.Value);
                }
                 

                try{
                    return _contextAccessor?.HttpContext?.User?.GetId<Guid>();
                }catch{
                    return null;
                }
            }
        }

        public string Name {
            get {

                if (_contextAccessor?.HttpContext?.User?.Identity != null) {
                    return _contextAccessor?.HttpContext?.User?.Identity?.Name;
                }

                return null;
            }
        }

        /// <summary>
        /// Check user to specific role
        /// </summary>
        /// <param name="role_name"></param>
        /// <returns></returns>
        public bool HasRole(string role_name) {

            if (string.IsNullOrWhiteSpace(role_name)) {
                return false;
            }

            return TestRole(_contextAccessor, role_name);
        }


        public static bool TestRole(IHttpContextAccessor context, string role) {

            if (context?.HttpContext?.User != null && context.HttpContext.User.HasClaim(ClaimTypes.Role, role)) {
                return true;
            }

            return false;
        }
    }
}
