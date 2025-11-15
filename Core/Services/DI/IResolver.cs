using Microsoft.Extensions.DependencyInjection;

namespace EzeePdf.Core.Services.DI
{
    public interface IResolver
    {
        T Get<T>() where T : class;
        IScopedService<T> GetScoped<T>() where T : class;
        IServiceScope Scope();
    }
}
