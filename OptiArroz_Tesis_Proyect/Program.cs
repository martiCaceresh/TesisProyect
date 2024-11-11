using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using OptiArroz_Tesis_Proyect.Data;
using OptiArroz_Tesis_Proyect.Data.DataAccess;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    // Configurar m�s opciones de Identity si es necesario
})
.AddDefaultTokenProviders()
.AddDefaultUI()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});


//Mapping Service
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<INotificationSenderDA, NotificationSenderDA>();
builder.Services.AddTransient<IRiceClassDA, RiceClassDA>();
builder.Services.AddTransient<IRiceGradeDA, RiceGradeDA>();
builder.Services.AddTransient<IRiceLotDA, RiceLotDA>();
builder.Services.AddTransient<IRiceSacksOutputDA, RiceSacksOutputDA>();
builder.Services.AddTransient<IRiceLotMovementDA, RiceLotMovementDA>();
builder.Services.AddTransient<IRiceClassificationDA, RiceClassificationDA>();
builder.Services.AddTransient<IZoneDA, ZoneDA>();
builder.Services.AddTransient<IUbicationDA, UbicationDA>();
builder.Services.AddTransient<IFirstConfigurationDA, FirstConfigurationDA>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Entry}/{id?}");

app.MapRazorPages();
IWebHostEnvironment env = app.Environment;
app.Run();
