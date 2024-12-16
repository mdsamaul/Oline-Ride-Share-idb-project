using System;

namespace Oline_Ride_Share_idb_project.Server.Services
{
    public class DistanceService
    {
        private const double EarthRadiusKm = 6371.0;  // Radius of the Earth in kilometers

        public double CalculateDistance(float sourceLat, float sourceLng, float destLat, float destLng)
        {
            // Convert degrees to radians
            double lat1 = DegreesToRadians(sourceLat);
            double lon1 = DegreesToRadians(sourceLng);
            double lat2 = DegreesToRadians(destLat);
            double lon2 = DegreesToRadians(destLng);

            // Calculate differences
            double deltaLat = lat2 - lat1;
            double deltaLon = lon2 - lon1;

            // Haversine formula
            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Distance in kilometers
            double distanceKm = EarthRadiusKm * c;

            // Convert to meters
            double distanceMeters = distanceKm * 1000;

            //return (float)distanceMeters;  // Return distance in meters
            return distanceKm;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
