using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services.Interfaces;

using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer.Data;
using RepositoryLayer.Repositories;
using ReviewSlotManager.Middlewares;
using ServiceLayer.Mappings;
using ServiceLayer.Services;
using ServiceLayer.Settings;

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
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Nhập JWT token. Ví dụ: Bearer {token}"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

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
        builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IGroupRepository, GroupRepository>();
        builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
        builder.Services.AddScoped<IReviewerSlotConfigRepository, ReviewerSlotConfigRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

        builder.Services.AddScoped<ISlotService, SlotService>();
        builder.Services.AddScoped<IReviewRoundService, ReviewRoundService>();
        builder.Services.AddScoped<IGroupSlotRegistrationService, GroupSlotRegistrationService>();
        builder.Services.AddScoped<IReviewerSlotRegistrationService, ReviewerSlotRegistrationService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ISemesterService, SemesterService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IGroupService, GroupService>();
        builder.Services.AddScoped<IGroupMemberService, GroupMemberService>();
        builder.Services.AddScoped<IReviewerSlotConfigService, ReviewerSlotConfigService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();

        // JWT Settings
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
        var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings are missing.");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });
        builder.Services.AddAuthorization();

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
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
