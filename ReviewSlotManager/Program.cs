
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using RepositoryLayer.Data;
using RepositoryLayer.Repositories;
using ReviewSlotManager.Middlewares;
using ServiceLayer.Mappings;
using ServiceLayer.Services;

namespace ReviewSlotManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        builder.Services.AddSingleton(mapperConfig.CreateMapper());

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnectionString' is missing.");

        builder.Services.AddDbContext<ReviewSlotDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddScoped<ISlotRepository, SlotRepository>();
        builder.Services.AddScoped<IReviewRoundRepository, ReviewRoundRepository>();
        builder.Services.AddScoped<IGroupSlotRegistrationRepository, GroupSlotRegistrationRepository>();
        builder.Services.AddScoped<IReviewerSlotRegistrationRepository, ReviewerSlotRegistrationRepository>();

        builder.Services.AddScoped<ISlotService, SlotService>();
        builder.Services.AddScoped<IReviewRoundService, ReviewRoundService>();
        builder.Services.AddScoped<IGroupSlotRegistrationService, GroupSlotRegistrationService>();
        builder.Services.AddScoped<IReviewerSlotRegistrationService, ReviewerSlotRegistrationService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ReviewSlotDbContext>();
            const int maxRetries = 12;
            for (var attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    dbContext.Database.Migrate();
                    DbSeeder.SeedAsync(dbContext).GetAwaiter().GetResult();
                    break;
                }
                catch (Exception ex) when (ex is SqlException or InvalidOperationException)
                {
                    if (attempt == maxRetries)
                    {
                        throw;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }
        }

        app.UseMiddleware<LogMiddleware>("ReviewSlotManager request");
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseStaticFiles();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ReviewSlotManager API v1");
            
            // Inject theme switcher script
            options.InjectJavascript("/swagger-theme-switcher.js");
        });

        app.UseHttpsRedirection();
        app.UseCors("AllowFrontend");
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
