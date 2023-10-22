using HowTo.Services;
using HowTo.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace HowTo.API.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<IVersionService, VersionService>();
        }
    }
}
