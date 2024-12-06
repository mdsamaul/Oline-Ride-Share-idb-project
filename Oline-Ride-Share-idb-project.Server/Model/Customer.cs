using Ride_Sharing_Project_isdb_bisew.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Customer : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public required string CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public required string CustomerNID { get; set; }
        public string? CustomerImage { get; set; }
        public float CustomerLaitude { get; set; }
        public float CustomerLongitude { get; set; }
        public virtual ICollection<Chat>? Chats { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }
        public virtual ICollection<RideBook>? RideBooks { get; set; }
    }

}
