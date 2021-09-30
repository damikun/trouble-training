using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedCore.Aplication.Interfaces;
using SharedCore.Aplication.Extensions;
using System;
using System.Linq;

namespace SharedCore.Aplication.Services {

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

        #nullable enable
        public Guid? UserId {

            get {
            
                try{
                    return _contextAccessor?.HttpContext?.User?.GetId<Guid>();
                }catch{
                    return null;
                }
            }
        }
        #nullable disable

        #nullable enable
        public ClaimsIdentity? Claims {

            get {
                
                try{
                    return _contextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
                }catch{
                    return null;
                }
            }
        }
        #nullable disable

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

        public string GetClaim(string type){

            try{
                return _contextAccessor?.HttpContext?.User?.Claims
                .Where(e => e.Type == type)
                .Select(e => e.Value)
                .SingleOrDefault();
            }catch{
                return null;
            }
        }

        public static bool TestRole(IHttpContextAccessor context, string role) {

            if (context?.HttpContext?.User != null && context.HttpContext.User.HasClaim(ClaimTypes.Role, role)) {
                return true;
            }

            return false;
        }
        
    }
}
