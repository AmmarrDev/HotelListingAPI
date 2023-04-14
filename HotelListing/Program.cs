using Serilog;
using System.ComponentModel.DataAnnotations;

namespace HotelListing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Information("Application is starting ...");
            // Add Serilog to the application
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(path: "D:\\Projects\\Api-Development\\HotelListingLogs\\myapplogs.log",
                outputTemplate: "{Timestamp: yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
                ).CreateLogger();

            builder.Host.UseSerilog();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Cross Origin Resource Sharing -- CORS
            // Allows or Restricts sharing of resources across domains

            builder.Services.AddCors(o =>
            {
                o.AddPolicy("AllowAllPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors("AllowAllPolicy");

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal("Application failed unexpectedly ..." + ex.Message.ToString());
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }
    }
}