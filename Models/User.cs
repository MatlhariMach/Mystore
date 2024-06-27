using Microsoft.AspNetCore.Identity;
using System.Data;


namespace Mystore.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
      
        
    }
}
