using HowTo.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HowTo.Services.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<IVersionService, VersionService>();
            services.AddTransient<IStringService, StringService>();
        }
    }
}
