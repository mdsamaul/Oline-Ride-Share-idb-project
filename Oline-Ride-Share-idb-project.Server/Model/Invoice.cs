using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Invoice : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }
        [Required]
        public DateTime PaymentTime { get; set; }
        public decimal Amount { get; set; }
        public string? Particular { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; } 
        [ForeignKey("PaymentMethod")]
        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; } 
        public virtual ICollection<Payment>? Payments { get; set; }
    }
}
