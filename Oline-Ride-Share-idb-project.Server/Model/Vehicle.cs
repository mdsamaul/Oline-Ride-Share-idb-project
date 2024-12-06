using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ride_Sharing_Project_isdb_bisew.Models;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Vehicle : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleId { get; set; }
        public required string VehicleBrand { get; set; }
        public required string VehicleModel { get; set; }
        public required string VehicleCapacity { get; set; }
        public required string VehicleRegistrationNo { get; set; }
        public required string VehicleChassisNo { get; set; }
        public required string VehicleLicence { get; set; }
        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }
        public virtual VehicleType? VehicleTypes { get; set; }
        public virtual ICollection<DriverVehicle>? DriverVehicles { get; set; }
    }
}
