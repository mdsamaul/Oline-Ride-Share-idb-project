using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ride_Sharing_Project_isdb_bisew.Models;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class DriverVehicle : BaseEntity
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverVehicleId { get; set; }
        [ForeignKey("Driver")]
        public int DriverId { get; set; }
        public virtual Driver? Drivers { get; set; }
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        public virtual Vehicle? Vehicles { get; set; }
        public virtual ICollection<RideBook>? RideBooks { get; set; }
    }
}
