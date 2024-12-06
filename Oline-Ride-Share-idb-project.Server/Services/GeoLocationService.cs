using Newtonsoft.Json;

namespace Oline_Ride_Share_idb_project.Server.Services
{
    public class GeoLocationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey = "f492dd2890ec415e9a12cd6be16a5c2f";  // OpenCage API Key

        public GeoLocationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Method to fetch Latitude and Longitude from the location name
        public async Task<(float Latitude, float Longitude)> GetCoordinatesFromLocation(string location)
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"https://api.opencagedata.com/geocode/v1/json?q={location}&key={_apiKey}";
            var response = await client.GetStringAsync(url);

            var geoData = JsonConvert.DeserializeObject<OpenCageGeocodeResponse>(response);
            if (geoData?.Results != null && geoData.Results.Count > 0)
            {
                var latitude = geoData.Results[0].Geometry.Lat;
                var longitude = geoData.Results[0].Geometry.Lng;
                return (latitude, longitude);
            }

            return (0f, 0f); // Return default values if no location is found
        }
    }

    public class OpenCageGeocodeResponse
    {
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}
