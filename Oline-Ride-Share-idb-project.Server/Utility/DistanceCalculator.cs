using System;

namespace Oline_Ride_Share_idb_project.Server.Utilities
{
    public static class DistanceCalculator
    {
        public static double CalculateDistance(float lat1, float lon1, float lat2, float lon2)
        {
            const double R = 6371e3; 
            var lat1Rad = ToRadians(lat1);
            var lat2Rad = ToRadians(lat2);
            var deltaLat = ToRadians(lat2 - lat1);
            var deltaLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        private static double ToRadians(float angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
