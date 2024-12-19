using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Chat : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatId { get; set; }
        public string? Message { get; set; }
        public DateTime ChatTime { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; } 
        [ForeignKey("DriverId")]
        public int DriverId { get; set; }
        public virtual Driver? Driver { get; set; } 
        public string? SenderType { get; set; } 
    }
}
