using SecureApp;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureDbContext();
builder.ConfigureIdentity();
builder.ConfigureServices();


builder.Services.AddControllersWithViews();





WebApplication app = builder.Build();

app.ConfigureEnv();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.ConfigureControllerRoutes();

app.MapRazorPages();

await app.Seed();

app.Run();
