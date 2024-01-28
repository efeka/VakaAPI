using Dapper;

namespace VakaAPI.Data
{
    public interface IDataContextDapper
    {
        Task<IEnumerable<T>> LoadDataAsync<T>(string sql);
        Task<T> LoadDataSingleAsync<T>(string sql);
        Task<bool> ExecuteSqlAsync(string sql);
        Task<int> ExecuteSqlWithRowCountAsync(string sql);
        Task<bool> ExecuteSqlWithParametersAsync(string sql, DynamicParameters parameters);
        Task<IEnumerable<T>> LoadDataWithParametersAsync<T>(string sql, DynamicParameters parameters);
        Task<T> LoadDataSingleWithParametersAsync<T>(string sql, DynamicParameters parameters);
    }
}
