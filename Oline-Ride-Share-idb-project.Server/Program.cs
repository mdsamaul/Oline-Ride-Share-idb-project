using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oline_Ride_Share_idb_project.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Add Controllers support
builder.Services.AddHttpClient(); // Register IHttpClientFactory for making HTTP requests
builder.Services.AddEndpointsApiExplorer(); // Add support for API documentation
builder.Services.AddSwaggerGen(); // Enable Swagger for API documentation

// Add database context with connection string
var cString = builder.Configuration.GetConnectionString("vehicleApp");
builder.Services.AddDbContext<DatabaseDbContext>(opt => opt.UseSqlServer(cString));

var app = builder.Build();

// Serve static files
app.UseDefaultFiles(); // Serve default files like index.html
app.UseStaticFiles();  // Serve other static files like CSS, JS, images

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();    // Enable Swagger for development
    app.UseSwaggerUI();  // Enable Swagger UI for testing API
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseAuthorization();    // Add Authorization middleware
app.MapControllers();      // Map controller endpoints

// Fallback for SPA routing
app.MapFallbackToFile("/index.html");

// Run the application
app.Run();
