using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class FareDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FareDetailsId { get; set; }
        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }
        public virtual VehicleType? VehicleType { get; set; }
        public decimal BaseFare { get; set; }
        public decimal TotalFare { get; set; }
    }

}
