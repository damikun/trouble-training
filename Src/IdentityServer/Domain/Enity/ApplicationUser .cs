using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Domain.Models
{

    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser(string userName) : base(userName)
        {

        }

        public ApplicationUser() : base()
        {

        }

    }

}