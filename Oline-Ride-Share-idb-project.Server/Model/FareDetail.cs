using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
//using Ride_Sharing_Project_isdb_bisew.Models;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class FareDetail : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FareDetailId { get; set; }
        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }
        public virtual VehicleType? VehicleType { get; set; }
        public decimal BaseFare { get; set; }
        public decimal TotalFare { get; set; }
    }


}
