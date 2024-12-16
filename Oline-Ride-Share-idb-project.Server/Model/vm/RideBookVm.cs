using System.ComponentModel.DataAnnotations.Schema;

namespace Oline_Ride_Share_idb_project.Server.Model.vm
{
    public class RideBookVm
    {
        public int RideBookId { get; set; }
        public float DistanceInMeters { get; set; }
        public decimal TotalFare { get; set; }
    }
}
