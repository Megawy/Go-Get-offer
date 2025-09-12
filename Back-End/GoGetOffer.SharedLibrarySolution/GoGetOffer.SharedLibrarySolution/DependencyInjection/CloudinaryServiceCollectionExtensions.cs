using GoGetOffer.SharedLibrarySolution.Interface;
using GoGetOffer.SharedLibrarySolution.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static GoGetOffer.SharedLibrarySolution.Interface.IGenericCloudinary;

namespace GoGetOffer.SharedLibrarySolution.DependencyInjection
{
    public static class CloudinaryServiceCollectionExtensions
    {
        public static IServiceCollection AddGenericCloudinary(this IServiceCollection services, IConfiguration config, string sectionName = "Cloudinary")
        {
            services.Configure<CloudinaryOptions>(config.GetSection(sectionName));
            services.AddSingleton<IGenericCloudinary, GenericCloudinaryService>();
            return services;
        }

        public static IServiceCollection AddGenericCloudinary(this IServiceCollection services, Action<CloudinaryOptions> configure)
        {
            services.Configure(configure);
            services.AddSingleton<IGenericCloudinary, GenericCloudinaryService>();
            return services;
        }
    }
}
