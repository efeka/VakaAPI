using Dapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using VakaAPI.Data;
using VakaAPI.Models;
using VakaAPI.Services;

namespace VakaAPI.Tests
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private EmployeeService _employeeService;
        private Mock<IDataContextDapper> _dapperMock;
        private Mock<ILogger<EmployeeService>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDataContextDapper>();
            _loggerMock = new Mock<ILogger<EmployeeService>>();
            _employeeService = new EmployeeService(_dapperMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dapperMock.Reset();
        }

        [Test]
        public async Task GetAllAsync_Success()
        {
            IEnumerable<Employee> testEmployees =
            [
                new() { EmployeeId = 1, TC = "00000000001", Name = "TestName1", Surname = "TestSurname1", EmployeeType = 1 },
                new() { EmployeeId = 2, TC = "00000000002", Name = "TestName2", Surname = "TestSurname2", EmployeeType = 2 },
                new() { EmployeeId = 3, TC = "00000000003", Name = "TestName3", Surname = "TestSurname3", EmployeeType = 3 }
            ];

            _dapperMock.Setup(x => x.LoadDataAsync<Employee>(It.IsAny<string>())).ReturnsAsync(testEmployees);

            var result = await _employeeService.GetAllAsync();

            ClassicAssert.IsNotNull(result);
            CollectionAssert.AreEqual(testEmployees, result);
        }

        [Test]
        public void GetAllAsync_ShouldThrowApplicationException()
        {
            var expectedException = new Exception();
            _dapperMock.Setup(x => x.LoadDataAsync<Employee>(It.IsAny<string>())).ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<ApplicationException>(_employeeService.GetAllAsync);

            ClassicAssert.IsInstanceOf<ApplicationException>(exception);
        }

        [TestCase(1, "00000000001", "TestName1", "TestSurname1", 1)]
        [TestCase(3, "00000000001", "TestName1", "TestSurname1", 3)]
        [TestCase(0, null, null, null, 0)]
        public async Task GetByIdAsync_Success(int id, string expectedTC, string expectedName,
            string expectedSurname, int expectedEmployeeType)
        {
            Employee testEmployee = new()
            {
                EmployeeId = id,
                TC = expectedTC,
                Name = expectedName,
                Surname = expectedSurname,
                EmployeeType = expectedEmployeeType
            };

            _dapperMock.Setup(x => x.LoadDataSingleWithParametersAsync<Employee>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(testEmployee);

            var result = await _employeeService.GetByIdAsync(id);
            ClassicAssert.AreEqual(testEmployee, result);
        }

        [Test]
        public void GetByIdAsync_ShouldThrowException()
        {
            int invalidEmployeeId = -1;

            _dapperMock.Setup(d => d.LoadDataSingleWithParametersAsync<Employee>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(new Exception());

            Assert.ThrowsAsync<ApplicationException>(async () => await _employeeService.GetByIdAsync(invalidEmployeeId));

            _dapperMock.Verify(d => d.LoadDataSingleWithParametersAsync<Employee>(It.IsAny<string>(), It.IsAny<DynamicParameters>()), Times.Once);
        }

        [Test]
        public async Task GetAllWithSalariesByDate_Success()
        {
            DateTime testDate = new DateTime(year: 2015, month: 5, day: 3);
            IEnumerable<EmployeeWithSalaryDto> testEmployees = new List<EmployeeWithSalaryDto>
            {
                new() {
                    EmployeeId = 1,
                    TC = "00000000001",
                    Name = "TestName1",
                    Surname = "TestSurname1",
                    EmployeeType = 1,
                    Salary = 1000.50,
                    SalaryDate = testDate
                },
                new() {
                    EmployeeId = 2,
                    TC = "00000000002",
                    Name = "TestName2",
                    Surname = "TestSurname2",
                    EmployeeType = 2,
                    Salary = 1500.75,
                    SalaryDate = testDate
                }
            };

            _dapperMock.Setup(x => x.LoadDataWithParametersAsync<EmployeeWithSalaryDto>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(testEmployees);

            var result = await _employeeService.GetAllWithSalariesByDate(testDate);

            ClassicAssert.IsNotNull(result);
            CollectionAssert.AreEqual(testEmployees, result);
        }

        [Test]
        public void GetAllWithSalariesByDate_ShouldThrowException()
        {
            _dapperMock.Setup(d => d.LoadDataWithParametersAsync<EmployeeWithSalaryDto>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(new Exception());

            Assert.ThrowsAsync<ApplicationException>(async () => await _employeeService.GetAllWithSalariesByDate(DateTime.Now));

            _dapperMock.Verify(d => d.LoadDataWithParametersAsync<EmployeeWithSalaryDto>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()), Times.Once);
        }

        [Test]
        public async Task GetWithSalariesById_Success()
        {
            IEnumerable<EmployeeWithSalaryDto> testEmployees =
            [
                new()
                {
                    EmployeeId = 1,
                    TC = "00000000001",
                    Name = "TestName1",
                    Surname = "TestSurname1",
                    EmployeeType = 1,
                    Salary = 100,
                    SalaryDate = DateTime.Now
                },
                new()
                {
                    EmployeeId = 1,
                    TC = "00000000001",
                    Name = "TestName1",
                    Surname = "TestSurname1",
                    EmployeeType = 1,
                    Salary = 200,
                    SalaryDate = DateTime.Now
                }
            ];

            _dapperMock.Setup(x => x.LoadDataWithParametersAsync<EmployeeWithSalaryDto>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(testEmployees);

            var result = await _employeeService.GetWithSalariesById(1);

            ClassicAssert.IsNotNull(result);
            CollectionAssert.AreEqual(testEmployees, result);
        }

        [Test]
        public void GetWithSalariesById_ShouldThrowException()
        {
            int invalidId = -1;

            _dapperMock.Setup(d => d.LoadDataWithParametersAsync<EmployeeWithSalaryDto>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(new Exception());

            Assert.ThrowsAsync<ApplicationException>(async () => await _employeeService.GetWithSalariesById(invalidId));

            _dapperMock.Verify(d => d.LoadDataWithParametersAsync<EmployeeWithSalaryDto>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()), Times.Once);
        }

        [Test]
        public async Task AddAsync_Success()
        {
            Employee testEmployee = new()
            {
                EmployeeId = 1,
                TC = "00000000001",
                Name = "Test",
                Surname = "Test",
                EmployeeType = 1
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(true);

            var result = await _employeeService.AddAsync(testEmployee);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task AddAsync_Fail()
        {
            Employee testEmployee = new()
            {
                EmployeeId = 1,
                TC = "00000000001",
                Name = "Test",
                Surname = "Test",
                EmployeeType = 1
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(false);

            var result = await _employeeService.AddAsync(testEmployee);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void AddAsync_ShouldThrowException()
        {
            Employee testEmployee = new()
            {
                EmployeeId = -1,
                TC = "00000000001",
                Name = "Test",
                Surname = "Test",
                EmployeeType = 1
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(new Exception());

            Assert.ThrowsAsync<ApplicationException>(async () => await _employeeService.AddAsync(testEmployee));

            _dapperMock.Verify(d => d.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_Success()
        {
            Employee testEmployee = new()
            {
                EmployeeId = 1,
                TC = "00000000001",
                Name = "Test",
                Surname = "Test",
                EmployeeType = 1
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(true);

            var result = await _employeeService.UpdateAsync(testEmployee);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAsync_Fail()
        {
            Employee testEmployee = new()
            {
                EmployeeId = 1,
                TC = "00000000001",
                Name = "Test",
                Surname = "Test",
                EmployeeType = 1
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(false);

            var result = await _employeeService.UpdateAsync(testEmployee);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void UpdateAsync_ShouldThrowException()
        {
            Employee testEmployee = new()
            {
                EmployeeId = -1,
                TC = "00000000001",
                Name = "Test",
                Surname = "Test",
                EmployeeType = 1
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(new Exception());

            Assert.ThrowsAsync<ApplicationException>(async () => await _employeeService.UpdateAsync(testEmployee));

            _dapperMock.Verify(d => d.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Success()
        {
            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(true);

            var result = await _employeeService.DeleteAsync(1);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_Fail()
        {
            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(false);

            var result = await _employeeService.DeleteAsync(1);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void DeleteAsync_ShouldThrowException()
        {
            int invalidId = -1;

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(new Exception());

            Assert.ThrowsAsync<ApplicationException>(async () => await _employeeService.DeleteAsync(invalidId));

            _dapperMock.Verify(d => d.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()), Times.Once);
        }
    }
}

