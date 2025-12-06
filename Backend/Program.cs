using Microsoft.AspNetCore.Builder; 
using Backend.Data;
using Backend.IServices;
using Backend.Repositories.Interfaces;
using Backend.Repositories;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using System.Security.Claims;
using Backend.Models;
using Backend.WebSockets;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting TOUR-WebSite Backend application");

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from appsettings.json
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddScoped<JwtTokenService>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; 
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Memory cache for in-memory rate limiting
builder.Services.AddMemoryCache();


var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role 
        };

    });

builder.Services.AddAuthorization();


builder.Services.AddScoped<UserNotificationRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<UserNotification>();
builder.Services.AddScoped<Notificationservices>();
// builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<ReportServices>();
// builder.Services.AddScoped<IReportService, ReportService>();
// builder.Services.AddScoped<TripPl>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MyWebSocketManager>(); 
builder.Services.AddSingleton<notificationSocket>(); 

var app = builder.Build();

// Use Serilog for request logging
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

// One-time helper: if set, re-save existing users so the ValueConverter
// for `User.PhoneNumber` runs and encrypts stored phone numbers.
if (Environment.GetEnvironmentVariable("ENCRYPT_EXISTING_USERS") == "true")
{
    using var scope = app.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Load all users and mark PhoneNumber as modified to trigger conversion
    var users = ctx.Users.ToList();
    foreach (var u in users)
    {
        if (u.PhoneNumber != null)
        {
            // reassign to mark modified
            u.PhoneNumber = u.PhoneNumber;
            ctx.Entry(u).Property(x => x.PhoneNumber).IsModified = true;
        }
    }
    ctx.SaveChanges();
}

app.UseWebSockets();

// Activity logging middleware: logs all HTTP requests with user info and timing
app.UseMiddleware<Backend.Middleware.ActivityLoggingMiddleware>();

// Rate limiting middleware: place before swagger/static so it catches swagger.json and static file requests
app.UseMiddleware<Backend.Middleware.RateLimitingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TOUR-WebSite API v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

    Log.Information("TOUR-WebSite Backend application shut down gracefully");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}