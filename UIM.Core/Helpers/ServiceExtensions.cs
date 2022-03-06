using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Sieve.Services;
using UIM.Core.Data;
using UIM.Core.Helpers.Mappers;
using UIM.Core.Helpers.SieveExtensions;
using UIM.Core.Services;
using UIM.Core.Services.Interfaces;
using UIM.Core.Models.Entities;
using UIM.Core.Common;
using System.Linq;

namespace UIM.Core.Helpers
{
    public static class ServiceExtensions
    {
        public static void AddAuthenticationExt(this IServiceCollection services)
        {
            services
                .AddAuthentication(_ =>
                {
                    _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(_ =>
                {
                    _.RequireHttpsMetadata = true;
                    _.SaveToken = true;
                    _.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,

                        ClockSkew = TimeSpan.Zero,
                        ValidIssuers = EnvVars.ValidLocations,
                        ValidAudiences = EnvVars.ValidLocations,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            EncryptHelpers.EncodeASCII(EnvVars.Jwt.Secret)),
                    };
                });
        }

        public static void AddControllersExt(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            });
        }

        public static void AddCorsExt(this IServiceCollection services)
        {
            services.AddCors(_ =>
            {
                _.AddPolicy("default", conf =>
                    conf.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(EnvVars.ValidLocations));
            });
        }

        public static void AddDIContainerExt(this IServiceCollection services)
        {
            services.AddScoped<SieveProcessor>();
            services.AddScoped<ISieveProcessor, AppSieveProcessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IIdeaService, IdeaService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ISubmissionService, SubmissionService>();
        }

        public static void AddDbContextExt(this IServiceCollection services, string localDbConnectionString)
        {
            if (EnvVars.CoreEnv == "development")
                services.AddDbContextPool<UimContext>(_ => _.UseSqlServer(localDbConnectionString));
            else
            {
                services.AddDbContextPool<UimContext>(_ =>
                {
                    _.UseNpgsql($@"
						Port={EnvVars.Pgsql.Port};
						Server={EnvVars.Pgsql.Host};
						Database={EnvVars.Pgsql.Db};
						User Id={EnvVars.Pgsql.UserId};
						Pooling={EnvVars.Pgsql.Pooling}
						sslmode={EnvVars.Pgsql.SslMode};
						Password={EnvVars.Pgsql.Password};
						Trust Server Certificate={EnvVars.Pgsql.TrustServer};
						Integrated Security={EnvVars.Pgsql.IntegratedSecurity};
					");
                });
            }
        }

        public static void AddIdentityExt(this IServiceCollection services)
        {
            if (EnvVars.CoreEnv == "development")
            {
                services
                    .AddIdentity<AppUser, IdentityRole>(_ =>
                    {
                        _.SignIn.RequireConfirmedEmail = false;
                        _.SignIn.RequireConfirmedAccount = false;
                        _.SignIn.RequireConfirmedPhoneNumber = false;
                    })
                    .AddEntityFrameworkStores<UimContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

                services.Configure<IdentityOptions>(_ =>
                {
                    _.Password.RequiredLength = 0;
                    _.Password.RequireDigit = false;
                    _.Password.RequiredUniqueChars = 0;
                    _.Password.RequireUppercase = false;
                    _.Password.RequireLowercase = false;
                    _.Password.RequireNonAlphanumeric = false;
                });
            }
            else
            {
                services
                    .AddIdentity<AppUser, IdentityRole>(_ =>
                    {
                        _.SignIn.RequireConfirmedEmail = false;
                        _.SignIn.RequireConfirmedAccount = true;
                        _.SignIn.RequireConfirmedPhoneNumber = false;
                    })
                    .AddEntityFrameworkStores<UimContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

                services.Configure<IdentityOptions>(_ =>
                {
                    _.Password.RequireDigit = true;
                    _.Password.RequiredUniqueChars = 0;
                    _.Password.RequireUppercase = false;
                    _.Password.RequireLowercase = false;
                    _.Password.RequiredLength = default;
                });
            }
        }

        public static void AddMapperExt(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(UserProfile),
                typeof(IdeaProfile),
                typeof(SubmissionProfile)
            );
        }

        public static void AddSwaggerExt(this IServiceCollection services)
        {
            services.AddSwaggerGen(_ =>
            {
                _.DocumentFilter<LowercaseDocumentFilter>();
                _.SwaggerDoc("v1", new OpenApiInfo { Title = "UIM", Version = "v1" });
                _.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                _.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme." +
                                  "\nEnter 'Bearer' [space] and then your token in the text input below." +
                                  "\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                _.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static async Task CreateRolesAndPwdUser(IServiceProvider serviceProvider, bool disable)
        {
            if (!disable)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

                // Initializing custom roles
                var roleNames = new List<string>
                {
                    EnvVars.System.Role.Staff,
                    EnvVars.System.Role.PwrUser,
                    EnvVars.System.Role.Manager,
                    EnvVars.System.Role.Supervisor,
                };
                foreach (var name in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(name);
                    if (!roleExist)
                        await roleManager.CreateAsync(new IdentityRole(name));
                }

                // Create a super user who will maintain the system
                var existingPwrUser = await userManager.FindByEmailAsync(EnvVars.System.PwrUserAuth.Email);
                if (existingPwrUser == null)
                {
                    var pwrUser = new AppUser
                    {
                        EmailConfirmed = true,
                        FullName = "Henry David",
                        Email = EnvVars.System.PwrUserAuth.Email,
                        UserName = EnvVars.System.PwrUserAuth.UserName,
                        CreatedDate = DateTime.UtcNow,
                    };
                    var createPowerUser = await userManager.CreateAsync(pwrUser,
                        EnvVars.System.PwrUserAuth.Password);

                    if (createPowerUser.Succeeded)
                        await userManager.AddToRoleAsync(pwrUser, EnvVars.System.Role.PwrUser);
                }
            }
        }
    }
}