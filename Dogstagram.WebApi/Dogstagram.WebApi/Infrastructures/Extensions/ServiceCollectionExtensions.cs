namespace Dogstagram.WebApi.Infrastructures.Extensions
{
    using Azure.Storage.Blobs;
    using Dogstagram.WebApi.Data;
    using Dogstagram.WebApi.Data.Models;
    using Dogstagram.WebApi.Features.Follow;
    using Dogstagram.WebApi.Features.Identity;
    using Dogstagram.WebApi.Features.Post;
    using Dogstagram.WebApi.Features.Profile;
    using Dogstagram.WebApi.Features.Search;
    using Dogstagram.WebApi.Infrastructures.Filters;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;

    public static class ServiceCollectionExtensions
    {
        public static ApplicationSettings GetApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSections = configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettings>(appSettingsSections);
            return appSettingsSections.Get<ApplicationSettings>();
        }

        public static StorageConnectionString GetStorageConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            var storageConnectionSection = configuration.GetSection("StorageConnectionString");
            services.Configure<StorageConnectionString>(storageConnectionSection);
            return storageConnectionSection.Get<StorageConnectionString>();
        }

        public static IServiceCollection AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<DogstagramDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

        public static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<User, UserRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
               .AddRoles<UserRole>()
               .AddEntityFrameworkStores<DogstagramDbContext>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, ApplicationSettings applicationSettings)
        {
            var key = Encoding.ASCII.GetBytes(applicationSettings.Secret!);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, StorageConnectionString storageConnectionString)
        {
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IFollowService, FollowService>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IPostService, PostService>();
            services.AddSingleton(s => 
                    new BlobServiceClient(storageConnectionString.BlobStorageConnectionString));
            return services;
        }

        public static void AddApiControllers(this IServiceCollection services)
            => services.AddControllers(options => options.Filters.Add<ModelOrNotFoundActionFilter>());

        public static IServiceCollection AddSwagger(this IServiceCollection services)
            => services.AddSwaggerGen();
    }
}
