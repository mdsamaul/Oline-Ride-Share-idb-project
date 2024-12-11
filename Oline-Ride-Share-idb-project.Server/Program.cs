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

<<<<<<< HEAD
// Add DbContext for SQL Server using the connection string from the configuration.
builder.Services.AddDbContext<DatabaseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("vehicleApp")));

// Configure Swagger for API documentation (optional, only in development)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

// Serve static files if you're using them
=======
// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context with connection string
var cString = builder.Configuration.GetConnectionString("vehicleApp");
builder.Services.AddDbContext<DatabaseDbContext>(opt => { opt.UseSqlServer(cString); });

var app = builder.Build();

// Serve static files
>>>>>>> 59ad6d4544f47f00f5b8c1b8726896d593a07457
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

<<<<<<< HEAD
// Fallback for SPA routing (optional, if using a frontend framework)
=======
// Fallback for SPA routing
>>>>>>> 59ad6d4544f47f00f5b8c1b8726896d593a07457
app.MapFallbackToFile("/index.html");

// Run the application
app.Run();
