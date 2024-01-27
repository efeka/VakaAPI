using System.Data;
using System.Linq.Expressions;
using Dapper;
using Microsoft.Data.SqlClient;
using VakaAPI.Data;
using VakaAPI.Models;

namespace VakaAPI.Services
{
    public class AttendenceService : IGenericService<Attendence>
    {
        private readonly DataContextDapper _dapper;
        private readonly ILogger<AttendenceService> _logger;

        public AttendenceService(DataContextDapper dapper, ILogger<AttendenceService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<IEnumerable<Attendence>> GetAllAsync()
        {
            string sql = "EXEC VakaSchema.sp_Attendences_GetAll";

            try
            {
                IEnumerable<Attendence> attendences = await _dapper.LoadDataAsync<Attendence>(sql);
                return attendences;
            }
            catch (SqlException ex)
            {
                string errorMessage = "An error occured while getting attendences.";
                _logger.LogError(ex, errorMessage);
                throw new ApplicationException(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException();
            }
        }

        public async Task<Attendence?> GetByIdAsync(int id)
        {
            return (await GetByEmployeeIdAsync(id)).First();
        }

        public async Task<IEnumerable<Attendence>> GetByEmployeeIdAsync(int id)
        {
            string sql = @"
                EXEC VakaSchema.sp_Attendences_GetByEmployeeId 
                @EmployeeId = @IdParam";

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@IdParam", id, DbType.Int32);

            try
            {
                return await _dapper.LoadDataWithParametersAsync<Attendence>(sql, sqlParameters);
            }
            catch (SqlException ex)
            {
                string errorMessage = $"An error occurred while getting the attendence with ID {id}.";
                _logger.LogError(ex, errorMessage);
                throw new ApplicationException(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException();
            }
        }

        public async Task<bool> AddAsync(Attendence entity)
        {
            string sql = @"
                EXEC VakaSchema.sp_Attendences_Insert
                    @EmployeeId = @IdParam,
                    @Year = @YearParam,
                    @Month = @MonthParam,
                    @TotalDaysWorked = @DaysParam,
                    @ExtraHoursWorked = @HoursParam";

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@IdParam", entity.EmployeeId, DbType.Int32);
            sqlParameters.Add("@YearParam", entity.Year, DbType.Int32);
            sqlParameters.Add("@MonthParam", entity.Month, DbType.Int32);
            sqlParameters.Add("@DaysParam", entity.TotalDaysWorked, DbType.Int32);
            sqlParameters.Add("@HoursParam", entity.ExtraHoursWorked, DbType.Int32);

            try
            {
                return await _dapper.ExecuteSqlWithParametersAsync(sql, sqlParameters);
            }
            catch (SqlException ex)
            {
                string errorMessage = $"An error occurred while inserting an attendence.";
                _logger.LogError(ex, errorMessage);
                throw new ApplicationException(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException();
            }
        }

        public async Task<bool> UpdateAsync(Attendence entity)
        {
            string sql = @"
                EXEC VakaSchema.sp_Attendences_UpdateEmployeeAttendence
                    @EmployeeId = @IdParam,
                    @Year = @YearParam,
                    @Month = @MonthParam,
                    @TotalDaysWorked = @DaysParam,
                    @ExtraHoursWorked = @HoursParam";

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@IdParam", entity.EmployeeId, DbType.Int32);
            sqlParameters.Add("@YearParam", entity.Year, DbType.Int32);
            sqlParameters.Add("@MonthParam", entity.Month, DbType.Int32);
            sqlParameters.Add("@DaysParam", entity.TotalDaysWorked, DbType.Int32);
            sqlParameters.Add("@HoursParam", entity.ExtraHoursWorked, DbType.Int32);

            try
            {
                return await _dapper.ExecuteSqlWithParametersAsync(sql, sqlParameters);
            }
            catch (SqlException ex)
            {
                string errorMessage = $"An error occurred while updating attendence with employee ID {entity.EmployeeId}.";
                _logger.LogError(ex, errorMessage);
                throw new ApplicationException(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "EXEC VakaSchema.sp_Attendences_DeleteByEmployeeId @EmployeeId = @IdParam";

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@IdParam", id, DbType.Int32);

            try
            {
                return await _dapper.ExecuteSqlWithParametersAsync(sql, sqlParameters);
            }
            catch (SqlException ex)
            {
                string errorMessage = $"An error occurred while deleting attendence with employee ID {id}.";
                _logger.LogError(ex, errorMessage);
                throw new ApplicationException(errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException();
            }
        }
    }
}
