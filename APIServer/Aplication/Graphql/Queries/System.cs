using System;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Queries {

    /// <summary>
    ///  Webhook Querys
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Query)]
    public class SystemQueries {

        /// <summary>
        /// Return server current date-time
        /// </summary>
        /// <returns>DateTime current date time</returns>
        public DateTime GetServerDateTime() => DateTime.Now;
    }
}
