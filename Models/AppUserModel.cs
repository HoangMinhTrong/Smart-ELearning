using Microsoft.AspNetCore.Identity;

namespace Smart_ELearning.Models
{
    public class AppUserModel : IdentityUser
    {
        public string FirtName { get; set; }
        public string LastName { get; set; }
    }
}