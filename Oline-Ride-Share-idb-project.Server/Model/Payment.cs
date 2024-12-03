using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ride_Sharing_Project_isdb_bisew.Models;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Payment : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        //[ForeignKey("PaymentMethod")]
        //public int PaymentMethodId { get; set; }
        //public virtual PaymentMethod? PaymentMethods { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? Status { get; set; }
        public virtual Invoice? Invoices { get; set; }
    }
}
