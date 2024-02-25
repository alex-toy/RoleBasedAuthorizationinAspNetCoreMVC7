using SecureApp.Data;

namespace SecureApp
{
    public static class WebApplicationExtension
    {
        public static void ConfigureEnv(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
        }

        public static void ConfigureControllerRoutes(this WebApplication app)
        {
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }

        public static async Task Seed(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
            }
        }
    }
}
