using EzeePdf.Core.Services.DI;

namespace EzeePdf.Services
{
    public class ScopedService<T>(IServiceProvider serviceProvider) : IScopedService<T>
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;
        private IServiceScope? scope;
        public T Service
        {
            get
            {
                scope = serviceProvider.CreateScope();
                return scope.ServiceProvider.GetService<T>()!;
            }
        }
        public void Dispose()
        {
            scope?.Dispose();
        }
    }
}
