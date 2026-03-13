
using AutoMapper;
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

        builder.Services.AddSingleton<InMemoryDataStore>();

        builder.Services.AddScoped<SlotRepository>();
        builder.Services.AddScoped<ReviewRoundRepository>();
        builder.Services.AddScoped<GroupSlotRegistrationRepository>();
        builder.Services.AddScoped<ReviewerSlotRegistrationRepository>();

        builder.Services.AddScoped<SlotService>();
        builder.Services.AddScoped<ReviewRoundService>();
        builder.Services.AddScoped<GroupSlotRegistrationService>();
        builder.Services.AddScoped<ReviewerSlotRegistrationService>();

        var app = builder.Build();

        app.UseMiddleware<LogMiddleware>("ReviewSlotManager request");
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseCors("AllowFrontend");
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
