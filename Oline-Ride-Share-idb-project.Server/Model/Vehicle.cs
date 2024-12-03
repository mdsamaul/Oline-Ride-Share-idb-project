using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class Vehicle
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
        public virtual VehicleType? VehicleType { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<DriverVehicle>? DriverVehicles { get; set; }
        public virtual ICollection<RideBook>? RideBooks { get; set; }
    }

}
