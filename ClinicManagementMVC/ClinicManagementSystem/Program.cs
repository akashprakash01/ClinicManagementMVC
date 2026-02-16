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
            builder.Services.AddScoped<IDoctorRepository, DoctorRepositoryImpl>();
            builder.Services.AddScoped<IDoctorService, DoctorServiceImpl>();
            builder.Services.AddScoped<ILabTechnicianRepository, LabTechnicianRepositoryImpl>();
            builder.Services.AddScoped<ILabTechnicianService, LabTechnicianServiceImpl>();
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
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}