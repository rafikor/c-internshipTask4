using System.ComponentModel.DataAnnotations;

namespace AppUsersManage.Models
{
    public class UsersStats
    {
        [DataType(DataType.Date)]
        public DateTime DateRegistered { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateLastVisited { get; set; }

        public string UserId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string UserName { get; set; }

        //[DataType()]
        public Status Status { get; set; }
    }
}
