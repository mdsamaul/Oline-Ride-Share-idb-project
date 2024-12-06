using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Company
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public virtual ICollection<Bank>? Banks { get; set; }
        public virtual ICollection<Driver>? Drivers { get; set; }
    }

}
