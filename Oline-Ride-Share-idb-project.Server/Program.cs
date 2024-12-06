using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.Run();
