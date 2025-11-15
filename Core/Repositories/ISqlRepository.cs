using System.Data;
using EzeePdf.Core.Model.Sql;

namespace EzeePdf.Core.Repositories
{
    public interface ISqlRepository
    {
        Task<SqlResponse<int>> ExecuteProc(SqlRequest request);
        Task ExecuteProc(string name, params Param[] parameters);
        Task<IEnumerable<T>> All<T>(string procName, string name, object parameter);
        Task<SqlResponse<IEnumerable<T>>> All<T>(SqlRequest request);
        Task<IEnumerable<T>> All<T>(string procName, params Param[] parameters);
        Task<SqlResponse<T>> Single<T>(SqlRequest request);
        Task<T?> Single<T>(string procName, bool query = false, params Param[] parameters);
        Task<SqlResponse<IDataReader>> GetReader(SqlRequest request);
        Task<IDataReader> GetReader(string procName, params Param[] parameters);
        Task<IDataReader> GetQueryReader(string query, params Param[] parameters);
        Task<bool> ExecuteBoolQuery(string query, Dictionary<string, object>? parameters = null);
    }
}
