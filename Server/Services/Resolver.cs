using EzeePdf.Core.Services.DI;

namespace EzeePdf.Services
{
    internal class Resolver : IResolver
    {
        private readonly IServiceProvider serviceProvider;
        internal Resolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public T Get<T>() where T : class
        {
            return serviceProvider.GetService<T>()!;
        }
        public IScopedService<T> GetScoped<T>() where T : class
        {
            return new ScopedService<T>(serviceProvider);
        }
        public IServiceScope Scope()
        {
            return serviceProvider.CreateScope();
        }
    }
}
