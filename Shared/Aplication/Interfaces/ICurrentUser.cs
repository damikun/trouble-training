using System;

namespace Shared.Aplication.Interfaces {

    /// <summary>
    /// Current user provider
    /// </summary>
    public interface ICurrentUser {

        /// <summary>
        /// Returns true if user exist in context
        /// </summary>
        bool Exist { get; }

        /// <summary>
        /// Returns curren user system Id
        /// </summary>
        Guid? UserId { get; }

        /// <summary>
        /// Returns useer name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Test user regarding to specific role
        /// </summary>
        /// <param name="role_name"></param>
        /// <returns></returns>
        bool HasRole(string role_name);

    }
   
}