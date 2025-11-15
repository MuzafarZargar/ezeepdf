namespace EzeePdf.Services
{
    public class InternalResolver
    {
        private readonly IServiceProvider serviceProvider;
        internal InternalResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public T Get<T>() where T : class
        {
            using (var scope = serviceProvider.CreateScope())
            {
                return scope.ServiceProvider.GetService<T>()!;
            }
        }
    }
}
