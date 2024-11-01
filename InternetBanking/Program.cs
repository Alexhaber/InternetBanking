using InternetBanking.Core.Application;
using InternetBanking.Infraestructure.Identity;
using InternetBanking.Infraestructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationLayer();
builder.Services.AddIdentityInfraestructureLayer(builder.Configuration);
builder.Services.AddPersistenceInfraestructureLayer(builder.Configuration);

var app = builder.Build();

var userId = await app.Services.SeedIdentityDbAsync();

if(userId != null)
{
	await app.Services.SeedProductDefaulClient(userId);
}

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
	pattern: "{controller=Account}/{action=Login}/{id?}");

await app.RunAsync();
