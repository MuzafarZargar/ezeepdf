using System.Data;
using Dapper;
using EzeePdf.Core.Model.Sql;
using Microsoft.Data.SqlClient;

namespace EzeePdf.Core.Repositories
{
    public abstract class SqlRepository(string connectionString) : ISqlRepository
    {
        private string connectionString = connectionString;
        public void SetConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
        }
        protected virtual IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
        public async Task<SqlResponse<int>> ExecuteProc(SqlRequest request)
        {
            var parameters = CreateParameters(request.Parameters);
            var response = await Connection.ExecuteAsync(request.ProcName, parameters, null, 0, CommandType.StoredProcedure);
            var sqlResponse = new SqlResponse<int>(response);
            FillOutputParameters(request, sqlResponse, parameters);
            return sqlResponse;
        }
        public async Task ExecuteProc(string name, params Param[] parameters)
        {
            var sqlParameters = CreateParameters(parameters);
            await Connection.ExecuteScalarAsync(name, sqlParameters, null, 0, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<T>> All<T>(string procName, string name, object parameter)
        {
            var p = new DynamicParameters();
            p.Add(name, parameter);

            return await Connection.QueryAsync<T>(procName, p, null, 0, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<T>> All<T>(string procName, params Param[] parameters)
        {
            var p = CreateParameters(parameters);
            return await Connection.QueryAsync<T>(procName, p, null, 0, CommandType.StoredProcedure);
        }
        public async Task<SqlResponse<IEnumerable<T>>> All<T>(SqlRequest request)
        {
            var parameters = CreateParameters(request.Parameters);
            var response = await Connection.QueryAsync<T>(request.ProcName, parameters, null, 0, CommandType.StoredProcedure);
            var sqlResponse = new SqlResponse<IEnumerable<T>>(response);
            FillOutputParameters(request, sqlResponse, parameters);
            return sqlResponse;
        }
        public async Task<SqlResponse<T>> Single<T>(SqlRequest request)
        {
            var parameters = CreateParameters(request.Parameters);
            var response = await Connection.QueryFirstOrDefaultAsync<T>(request.ProcName, parameters, null, 0, CommandType.StoredProcedure);
            var sqlResponse = new SqlResponse<T>(response);
            FillOutputParameters(request, sqlResponse, parameters);
            return sqlResponse;
        }
        public async Task<T?> Single<T>(string procName, bool query = false, params Param[] parameters)
        {
            var p = CreateParameters(parameters);
            var response = await Connection.QueryFirstOrDefaultAsync<T>(procName, p, null, 0, query ? CommandType.Text : CommandType.StoredProcedure);
            return response;
        }
        public async Task<SqlResponse<IDataReader>> GetReader(SqlRequest request)
        {
            var parameters = CreateParameters(request.Parameters);
            var response = await Connection.ExecuteReaderAsync(request.ProcName, parameters, null, 0, CommandType.StoredProcedure);
            var sqlResponse = new SqlResponse<IDataReader>(response);
            FillOutputParameters(request, sqlResponse, parameters);
            return sqlResponse;
        }
        public async Task<IDataReader> GetReader(string procName, params Param[] parameters)
        {
            return await Connection.ExecuteReaderAsync(procName, CreateParameters(parameters), null, 0, CommandType.StoredProcedure);
        }
        public Task<IDataReader> GetQueryReader(string query, params Param[] parameters)
        {
            return Connection.ExecuteReaderAsync(query, CreateParameters(parameters), commandType: CommandType.Text);
        }
        public async Task<bool> ExecuteBoolQuery(string query, Dictionary<string, object>? parameters = null)
        {
            var result = false;
            var response = await Connection.ExecuteScalarAsync(query, CreateParameters(parameters), commandType: CommandType.Text);
            if (response != null)
            {
                result = bool.TryParse(response.ToString(), out var _);
            }
            return result;
        }
        private void FillOutputParameters<T>(SqlRequest request, SqlResponse<T> sqlResponse, DynamicParameters? parameters)
        {
            if (parameters is not null && request.HasOuputParameters && request.Parameters is not null)
            {
                sqlResponse.Output = new Dictionary<string, object>();

                foreach (var p in request.Parameters)
                {
                    if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.ReturnValue)
                    {
                        sqlResponse.Output.Add(p.Name, parameters.Get<object>(p.Name));
                    }
                }
            }
        }

        private DynamicParameters? CreateParameters(IEnumerable<Param>? parameters)
        {
            DynamicParameters? dynamicParameters = null;

            if (parameters?.Any() == true)
            {
                dynamicParameters = new DynamicParameters();

                foreach (var p in parameters)
                {
                    if (p.IsTable)
                    {
                        dynamicParameters.Add(p.Name, (p.Value as DataTable)!.AsTableValuedParameter(), p.Type);
                    }
                    else
                    {
                        dynamicParameters.Add(p.Name, p.Value, p.Type, p.Direction, size: p.Size);
                    }
                }
            }

            return dynamicParameters;
        }
        private DynamicParameters? CreateParameters(Dictionary<string, object>? parameters)
        {
            DynamicParameters? dynamicParameters = null;

            if (parameters?.Any() == true)
            {
                dynamicParameters = new DynamicParameters();

                foreach (var p in parameters)
                {
                    dynamicParameters.Add(p.Key, p.Value);
                }
            }

            return dynamicParameters;
        }
    }
}
