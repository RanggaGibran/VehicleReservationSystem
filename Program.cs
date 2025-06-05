using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Data;
using VehicleReservationSystem.Models;
using VehicleReservationSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging for production
if (!builder.Environment.IsDevelopment())
{
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
}

// Configure port for Heroku
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllersWithViews();

// Database configuration
string connectionString;
if (builder.Environment.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db";
}
else
{
    // For production, use a simple SQLite database in the app directory
    connectionString = "Data Source=app.db";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure authentication options
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IApprovalService, ApprovalService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

// Startup validation
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"Port: {port}");
Console.WriteLine($"Application starting...");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Remove HSTS for Heroku - they handle HTTPS at load balancer level
    // app.UseHsts();
}

// Remove HTTPS redirection for Heroku deployment
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add a simple health check endpoint
app.MapGet("/health", async (AppDbContext context) => 
{
    try 
    {
        await context.Database.CanConnectAsync();
        return Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Database connection failed: {ex.Message}");
    }
});

// HAPUS atau KOMENTARI kode ini:
/*
app.MapGet("/", () => Results.Ok(new { 
    message = "Vehicle Reservation System is running",
    timestamp = DateTime.UtcNow,
    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
}));
*/

// Database initialization - more robust for production
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            
            // Ensure database is created
            context.Database.EnsureCreated();
            
            // Only run migrations in development
            if (app.Environment.IsDevelopment())
            {
                context.Database.Migrate();
            }

            // Initialize data if needed
            var roleManager = services.GetService<RoleManager<IdentityRole>>();
            var userManager = services.GetService<UserManager<ApplicationUser>>();
            
            if (roleManager != null && userManager != null)
            {
                await DbInitializer.Initialize(services);
                logger.LogInformation("Database initialized successfully.");
            }
        }
        catch (Exception ex)
        {
            var logger2 = services.GetRequiredService<ILogger<Program>>();
            logger2.LogError(ex, "An error occurred while initializing the database.");
            
            // In production, don't fail startup due to database issues
            if (app.Environment.IsDevelopment())
            {
                throw;
            }
        }
    }
}
catch (Exception ex)
{
    // Fallback logging if services aren't available
    Console.WriteLine($"Failed to initialize database: {ex.Message}");
    if (app.Environment.IsDevelopment())
    {
        throw;
    }
}

app.Run();