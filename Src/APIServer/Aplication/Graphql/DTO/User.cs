
namespace APIServer.Aplication.GraphQL.DTO
{

    public class GQL_User
    {
        public GQL_User()
        {

        }

        // <summary>
        /// ID
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        ///  Name
        /// </summary>
        public string Name { get; set; }

#nullable enable
        /// <summary>
        ///  Name
        /// </summary>
        public string SessionId { get; set; }


        /// <summary>
        ///  Email
        /// </summary>
        public string? Email { get; set; }
#nullable disable
    }
}


