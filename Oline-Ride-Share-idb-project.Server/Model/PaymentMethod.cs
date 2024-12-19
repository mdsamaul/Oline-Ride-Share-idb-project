using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class PaymentMethod : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentMethodId { get; set; }
        public string? MethodType { get; set; }
        public virtual ICollection<Invoice>? Invoices { get; set; }
    }
}
