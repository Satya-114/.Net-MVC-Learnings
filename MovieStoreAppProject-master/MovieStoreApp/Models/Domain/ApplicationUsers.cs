using Microsoft.AspNetCore.Identity;

namespace MovieStoreApp.Models.Domain
{
    public class ApplicationUsers : IdentityUser
    {
        public string Name { get; set; }
    }
}
