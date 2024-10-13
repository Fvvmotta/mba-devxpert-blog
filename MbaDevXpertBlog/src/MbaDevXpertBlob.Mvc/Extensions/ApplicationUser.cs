using Microsoft.AspNetCore.Identity;

namespace MMA_Claims.Api.Extensions
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
