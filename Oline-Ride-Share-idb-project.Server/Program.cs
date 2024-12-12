using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Register the necessary services.
builder.Services.AddControllers();

// Register IHttpClientFactory
builder.Services.AddHttpClient();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Fallback for SPA routing (optional, if using a frontend framework)
app.MapFallbackToFile("/index.html");

// Run the application
app.Run();
