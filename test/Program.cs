using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using test.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<testContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("testContext") ?? throw new InvalidOperationException("Connection string 'testContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<testContext>();
    DbInitializer.Initialize(context);
}
app.Run();
