using applicationmvc.Context;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentNullException(nameof(connectionString), "Connection string 'DefaultConnection' not found.");
}

builder.Services.AddLinqToDBContext<ApplicationDbContext>((sp, options) =>
{
    return options.UsePostgreSQL(connectionString);
});

builder.Services.AddScoped<DataConnection, ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
