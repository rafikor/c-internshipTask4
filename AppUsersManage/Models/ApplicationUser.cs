using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppUsersManage.Models
{
    public class ApplicationUser : IdentityUser
    {
        [DataType(DataType.Date)]
        public DateTime DateRegistered { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateLastLogin { get; set; }

        public Status Status { get; set; }

        public ApplicationUser():base()
        {
            this.DateRegistered = DateTime.UtcNow.Date;
            this.DateLastLogin = DateTime.UtcNow.Date;
            this.Status = Status.Active;
        }
    }

    public enum Status {Active, Blocked};
}
