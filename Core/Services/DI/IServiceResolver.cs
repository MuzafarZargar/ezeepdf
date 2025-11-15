using Microsoft.AspNetCore.Http;

namespace EzeePdf.Core.Services.DI
{
    public interface IServiceResolver
    {
        T Get<T>()
            where T : class;
        HttpContext? HttpContext { get; }
    }
}
