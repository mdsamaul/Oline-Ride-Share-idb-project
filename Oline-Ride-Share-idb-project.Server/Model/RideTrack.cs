using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oline_Ride_Share_idb_project.Server.Model
{
    public class RideTrack
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RideTrackId { get; set; }
        [ForeignKey("RideBook")]
        public int RideBookId { get; set; }
        public virtual RideBook? RideBooks { get; set; }
        public float RideTrackLaitude { get; set; }
        public float RideTrackLongtiude { get; set; }
        public DateTime Timestamp { get; set; }
        public int Distance { get; set; }
        public DateTime TrackTime { get; set; }
    }
}
