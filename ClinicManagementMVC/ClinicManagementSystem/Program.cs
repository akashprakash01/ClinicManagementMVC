using ClinicManagementSystem.Repository;
using ClinicManagementSystem.Service;

namespace ClinicManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("ConnStringMVC");
            builder.Services.AddSingleton(connectionString);

            builder.Services.AddScoped<IUserRepository, UserServiceRepoImp>();
            builder.Services.AddScoped<IUserServic, UserServiceImp>();
            builder.Services.AddScoped<IReceptionistRepository, ReceptionistRepository>();
            builder.Services.AddScoped<IReceptionistService, ReceptionistService>();


            builder.Services.AddSession();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}