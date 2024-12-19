using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class VehicleType : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleTypeId { get; set; }
        public string? VehicleTypeName { get; set; }
        public decimal PerKmFare { get; set; }
        public virtual ICollection<FareDetail>? FareDetails { get; set; }
        public virtual ICollection<Vehicle>? Vehicles { get; set; }
    }
}
