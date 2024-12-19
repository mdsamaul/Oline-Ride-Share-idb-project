//using Ride_Sharing_Project_isdb_bisew.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Customer : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        [Required] 
        public string CustomerPhoneNumber { get; set; }
        [EmailAddress] 
        public string? CustomerEmail { get; set; }
        [Required] 
        public string CustomerNID { get; set; }
        public string? CustomerImage { get; set; }
        [Range(-90, 90)] 
        public float CustomerLatitude { get; set; }
        [Range(-180, 180)]
        public float CustomerLongitude { get; set; }
        public virtual ICollection<Chat>? Chats { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<RideBook>? RideBooks { get; set; }
    }
}
