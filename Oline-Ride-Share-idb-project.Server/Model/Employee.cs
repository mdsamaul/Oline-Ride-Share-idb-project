using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Employee : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public bool IsLive { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public virtual ICollection<Chat>? Chats { get; set; }
    }
}
