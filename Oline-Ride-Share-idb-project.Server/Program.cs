using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// Register the necessary services.
=======
// Add services to the container.
>>>>>>> 59ad6d4544f47f00f5b8c1b8726896d593a07457
builder.Services.AddControllers();

// Register IHttpClientFactory
builder.Services.AddHttpClient();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context with connection string
var cString = builder.Configuration.GetConnectionString("vehicleApp");
builder.Services.AddDbContext<DatabaseDbContext>(opt => { opt.UseSqlServer(cString); });

var app = builder.Build();

// Serve static files
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Fallback for SPA routing
app.MapFallbackToFile("/index.html");

// Run the application
app.Run();
