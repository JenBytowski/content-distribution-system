namespace DistributionSystemApi
{
    using DistributionSystemApi.ConfigurationExtensions;
    using DistributionSystemApi.MailLibrary;
    using Microsoft.AspNetCore.Builder;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDBContext(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddMailServices(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("AllowAnyOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAuthorization();
            app.MapRazorPages();

            app.Run();
        }
    }
}
