using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using ZavrsniSeminarskiRad.Data;
using ZavrsniSeminarskiRad.Models;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Services.Implementations;
using ZavrsniSeminarskiRad.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Jti;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddOpenApiDocument();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy.AllowAll, builder => builder.WithOrigins("https://localhost:8261")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed((host) => true));
});

builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddControllersWithViews();



builder.Services.AddSingleton<IIdentityService, IdentityService>();

builder.Services.AddScoped<ISharedService, SharedService>();
builder.Services.AddTransient<IAppUserService, AppUserService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IFileSaveService, FileSaveService>();
builder.Services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, QueueProcessor>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseOpenApi();
app.UseSwaggerUi3();
app.UseReDoc();

app.UseRouting();
app.UseCors(CorsPolicy.AllowAll);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Services.GetService<IIdentityService>();

app.Run();
