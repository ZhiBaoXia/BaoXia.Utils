var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



BaoXia.Utils.Environment.InitializeBeforeBuildApplication(
	"BaoXia.Utils.Test.DotNet7.MVC",
	BaoXia.Utils.Environment.GetEnvironmentNameWith_ASPNETCORE_ENVIRONMENT(),
	"0123456789",
	"/ConfigFiles",
	"/LogFiles");
////////////////////////////////////////////////

var app = builder.Build();

////////////////////////////////////////////////
BaoXia.Utils.Environment.InitializeAfterBuildApplication(
	app);



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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
