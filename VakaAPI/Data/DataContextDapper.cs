using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace VakaAPI.Data
{
    public class DataContextDapper : IDataContextDapper
    {
        private readonly IConfiguration _config;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IDbConnection> GetOpenConnectionAsync()
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            return connection;
        }

        public async Task<IEnumerable<T>> LoadDataAsync<T>(string sql)
        {
            using var dbConnection = await GetOpenConnectionAsync();
            return await dbConnection.QueryAsync<T>(sql);
        }

        public async Task<T> LoadDataSingleAsync<T>(string sql)
        {
            using var dbConnection = await GetOpenConnectionAsync();
            return await dbConnection.QuerySingleAsync<T>(sql);
        }

        public async Task<bool> ExecuteSqlAsync(string sql)
        {
            using var dbConnection = await GetOpenConnectionAsync();
            return await dbConnection.ExecuteAsync(sql) > 0;
        }

        public async Task<int> ExecuteSqlWithRowCountAsync(string sql)
        {
            using var dbConnection = await GetOpenConnectionAsync();
            return await dbConnection.ExecuteAsync(sql);
        }

        public async Task<bool> ExecuteSqlWithParametersAsync(string sql, DynamicParameters parameters)
        {
            using var dbConnection = await GetOpenConnectionAsync();
            return await dbConnection.ExecuteAsync(sql, parameters) > 0;
        }

        public async Task<IEnumerable<T>> LoadDataWithParametersAsync<T>(string sql, DynamicParameters parameters)
        {
            using var dbConnection = await GetOpenConnectionAsync();
            return await dbConnection.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> LoadDataSingleWithParametersAsync<T>(string sql, DynamicParameters parameters)
        {
            using var dbConnection = await GetOpenConnectionAsync();
            return await dbConnection.QuerySingleAsync<T>(sql, parameters);
        }
    }
}
