using System;
using System.Security.Claims;

namespace SharedCore.Aplication.Interfaces {

    /// <summary>
    /// Current user provider
    /// </summary>
    public interface ICurrentUser {

        /// <summary>
        /// Returns true if user exist in context
        /// </summary>
        bool Exist { get; }
#nullable enable
        /// <summary>
        /// Returns curren user system Id
        /// </summary>
        Guid? UserId { get; }
#nullable disable
        /// <summary>
        /// Returns user name
        /// </summary>
        string Name { get; }

#nullable enable
        /// <summary>
        /// Returns user claims
        /// </summary>
        ClaimsIdentity? Claims  { get; }
#nullable disable

        /// <summary>
        /// Get specific claim value
        /// </summary>
        string GetClaim(string type);

        /// <summary>
        /// Test user regarding to specific role
        /// </summary>
        /// <param name="role_name"></param>
        /// <returns></returns>
        bool HasRole(string role_name);

    }
   
}