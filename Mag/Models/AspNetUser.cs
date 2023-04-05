using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Mag.Models
{
    public class AspNetUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public DateTime? DOB { get; set; }
        public Genders? Gender { get; set; }
    }
    public enum Genders
    {
        Other,
        Male,
        Female
    }
}
