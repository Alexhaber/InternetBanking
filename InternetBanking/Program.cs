using InternetBanking.Infraestructure.Identity;
using InternetBanking.Infraestructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddIdentityInfraestructureLayer(builder.Configuration);
builder.Services.AddPersistenceInfraestructureLayer(builder.Configuration);

var app = builder.Build();

await app.Services.SeedIdentityDbAsync();
await app.Services.SeedSavingAccountDefaulClient();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
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
	pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
