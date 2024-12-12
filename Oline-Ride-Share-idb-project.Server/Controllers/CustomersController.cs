using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Model;
using Oline_Ride_Share_idb_project.Server.Data;
using Newtonsoft.Json;
using System.Net.Http;

namespace Oline_Ride_Share_idb_project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory; // For HttpClient

        // Constructor to inject DatabaseDbContext and IHttpClientFactory
        public CustomersController(DatabaseDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory; // Inject IHttpClientFactory
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            // If latitude or longitude is not provided, fetch it using IP Geolocation API
            if (customer.CustomerLatitude == 0 && customer.CustomerLongitude == 0)
            {
                var client = _httpClientFactory.CreateClient();

                // Replace with your actual IP Geolocation API URL and your API Key
                string apiUrl = $"https://api.ipgeolocation.io/ipgeo?apiKey=f492dd2890ec415e9a12cd6be16a5c2f";

                try
                {
                    // Make HTTP GET request to the API
                    var response = await client.GetStringAsync(apiUrl);
                    var geoData = JsonConvert.DeserializeObject<GeoLocationResponse>(response);

                    // Set the latitude and longitude from the API response
                    customer.CustomerLatitude = geoData.Latitude;
                    customer.CustomerLongitude = geoData.Longitude;
                }
                catch (Exception ex)
                {
                    // Handle error (e.g., API call failure)
                    return BadRequest("Could not retrieve location information.");
                }
            }

            // Add the customer to the database
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Return the newly created customer
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // Additional action to retrieve customer by ID (optional)
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }
    }

    // Create a class for the IP Geolocation API response format
    public class GeoLocationResponse
    {
        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }
    }
}
