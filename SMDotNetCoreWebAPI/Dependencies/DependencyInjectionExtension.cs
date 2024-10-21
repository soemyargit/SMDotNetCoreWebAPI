using Microsoft.Extensions.DependencyInjection;
using SMDotNetCoreWebAPI.Services;

namespace SMDotNetCoreWebAPI.Dependencies
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection serviecs)
        {
            return serviecs.AddScoped<DapperService>();
        }
    }
}
