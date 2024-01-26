using System.Data;
using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using VakaAPI.Data;
using VakaAPI.Models;

namespace VakaAPI.Services
{
    public class EmployeeService : IGenericService<Employee>
    {
        private readonly DataContextDapper _dapper;
        private readonly IMapper _mapper;

        public EmployeeService(DataContextDapper dapper)
        {
            _dapper = dapper;

            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmployeeToAddDto, Employee>();
            }));
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            string sql = "EXEC VakaSchema.sp_Employees_GetAll";

            try
            {
                IEnumerable<Employee> employees = await _dapper.LoadDataAsync<Employee>(sql);
                return employees;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                throw new ApplicationException("An error occured while getting employees.");
            }
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            string sql = "EXEC VakaSchema.sp_Employees_GetById @EmployeeId = @IdParam";

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@IdParam", id, DbType.Int32);

            try
            {
                return await _dapper.LoadDataSingleWithParametersAsync<Employee>(sql, sqlParameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                throw new ApplicationException($"An error occurred while getting the employee with ID {id}.");
            }
        }

        public async Task<bool> AddAsync(EmployeeToAddDto employeeDto)
        {
            Employee employee = GetEmployeeFromDtoMapping(employeeDto);
            return await AddAsync(employee);
        }

        public async Task<bool> AddAsync(Employee entity)
        {
            string sql = @"
                EXEC VakaSchema.sp_Employees_Insert
                    @TC = @TCParam,
                    @Name = @NameParam,
                    @Surname = @SurnameParam,
                    @EmployeeType = @EmployeeTypeParam";

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@TCParam", entity.TC, DbType.String);
            sqlParameters.Add("@NameParam", entity.Name, DbType.String);
            sqlParameters.Add("@SurnameParam", entity.Surname, DbType.String);
            sqlParameters.Add("@EmployeeTypeParam", entity.EmployeeType, DbType.Int32);

            try
            {
                return await _dapper.ExecuteSqlWithParametersAsync(sql, sqlParameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                throw new ApplicationException($"An error occurred while inserting an employee.");
            }
        }

        public async Task<bool> UpdateAsync(EmployeeToAddDto employeeDto, int employeeId)
        {
            Employee employee = GetEmployeeFromDtoMapping(employeeDto);
            employee.EmployeeId = employeeId;
            return await UpdateAsync(employee);
        }

        public async Task<bool> UpdateAsync(Employee entity)
        {
            string sql = @"
                EXEC VakaSchema.sp_Employees_Update
                    @EmployeeId = @IdParam,
                    @TC = @TCParam,
                    @Name = @NameParam,
                    @Surname = @SurnameParam,
                    @EmployeeType = @EmployeeTypeParam";

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@IdParam", entity.EmployeeId, DbType.Int32);
            sqlParameters.Add("@TCParam", entity.TC, DbType.String);
            sqlParameters.Add("@NameParam", entity.Name, DbType.String);
            sqlParameters.Add("@SurnameParam", entity.Surname, DbType.String);
            sqlParameters.Add("@EmployeeTypeParam", entity.EmployeeType, DbType.Int32);

            try
            {
                return await _dapper.ExecuteSqlWithParametersAsync(sql, sqlParameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                throw new ApplicationException($"An error occurred while updating employee with ID {entity.EmployeeId}.");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            string sql = "EXEC VakaSchema.sp_Employees_Delete @EmployeeId = @IdParam";

            DynamicParameters sqlParameters = new();
            sqlParameters.Add("@IdParam", id, DbType.Int32);

            try
            {
                return await _dapper.ExecuteSqlWithParametersAsync(sql, sqlParameters);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                throw new ApplicationException($"An error occurred while deleting employee with ID {id}.");
            }
        }

        private Employee GetEmployeeFromDtoMapping(EmployeeToAddDto employeeDto)
        {
            return _mapper.Map<Employee>(employeeDto);
        }
    }
}
