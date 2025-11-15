namespace EzeePdf.Core.Services.DI
{
    public interface IScopedService<T> : IDisposable
    {
        public T Service { get; }
    }
}
