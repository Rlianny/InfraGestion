using System;
using System.Text;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add services to the container.
        InjectInfrastructure(builder);
        InjectApplication(builder);
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        // Configure Swagger/OpenAPI to support JWT
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = "InfraGestion API", Version = "v1" }
            );

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Ingrese 'Bearer' seguido de un espacio y el token JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                }
            );

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                }
            );
        });

        //  Config JWT Authentication
        builder
            .Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)
                    ),
                };
            });

        // Configure Authorization Policies based on Roles
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Administrator"));
            options.AddPolicy("RequireTechnicianRole", policy => policy.RequireRole("Technician"));
            options.AddPolicy(
                "ManagementRoles",
                policy => policy.RequireRole("Administrator", "SectionManager")
            );
            options.AddPolicy(
                "CanManageDevices",
                policy => policy.RequireRole("Administrator", "SectionManager", "Director")
            );
            options.AddPolicy(
                "CanPerformMaintenance",
                policy => policy.RequireRole("Administrator", "Technician")
            );
        });

        // Configure HTTP for Blazor // DON'T DELETE
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();

    }

    private static void InjectInfrastructure(WebApplicationBuilder builder)
    {
        // Database Context
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        // Unit of Work DON'T DELETE
        builder.Services.AddScoped<IUnitOfWork>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

        // Repositories
        builder.Services.AddScoped<ISectionRepository, SectionRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMaintenanceRecordRepository, MaintenanceRepository>();
        builder.Services.AddScoped<
            IReceivingInspectionRequestRepository,
            ReceivingInspectionRequestRepository
        >();
        builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddScoped<
            IDecommissioningRequestRepository,
            DecommissioningRequestRepository
        >();
        builder.Services.AddScoped<ITransferRepository, TransferRepository>();
        builder.Services.AddScoped<IRejectionRepository, RejectionRepository>();
        builder.Services.AddScoped<IPerformanceRatingRepository, PerformanceRatingRepository>();
        builder.Services.AddScoped<IDecommissioningRepository, DecommissioningRepository>();
        builder.Services.AddScoped<ITechnicianRepository, TechnicianRepository>();
        // Infrastructure Services (Authentication & Security)
        builder.Services.AddScoped<IPasswordHasher, Infrastructure.Services.PasswordHasher>();
        builder.Services.AddScoped<IJwtTokenGenerator, Infrastructure.Services.JwtTokenGenerator>();
    }

    private static void InjectApplication(WebApplicationBuilder builder)
    {
        // AutoMapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Application Services
        builder.Services.AddScoped<IInventoryService, InventoryService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IUserManagementService, UserManagementService>();
        builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
        builder.Services.AddScoped<IDecommissioningService, DecommissioningService>();
        builder.Services.AddScoped<IPersonnelService, PersonnelService>();
        builder.Services.AddScoped<ITransferService, TransferService>();
        builder.Services.AddScoped<IOrgManagementService, OrgManagmentService>();
    }
}

