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
    public class AttendenceServiceTests
    {
        private AttendenceService _attendenceService;
        private Mock<IDataContextDapper> _dapperMock;
        private Mock<ILogger<AttendenceService>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _dapperMock = new Mock<IDataContextDapper>();
            _loggerMock = new Mock<ILogger<AttendenceService>>();
            _attendenceService = new AttendenceService(_dapperMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dapperMock.Reset();
        }

        [Test]
        public async Task GetAllAsync_Success()
        {
            IEnumerable<Attendence> testData =
            [
                new() { EmployeeId = 1, Year = 2015, Month = 5, TotalDaysWorked = 20, ExtraHoursWorked = 20 },
                new() { EmployeeId = 1, Year = 2015, Month = 6, TotalDaysWorked = 10, ExtraHoursWorked = 10 },
                new() { EmployeeId = 2, Year = 2015, Month = 5, TotalDaysWorked = 0, ExtraHoursWorked = 0 }
            ];

            _dapperMock.Setup(x => x.LoadDataAsync<Attendence>(It.IsAny<string>()))
                .ReturnsAsync(testData);

            var result = await _attendenceService.GetAllAsync();

            ClassicAssert.IsNotNull(result);
            CollectionAssert.AreEqual(testData, result);
        }

        [Test]
        public void GetAllAsync_ShouldThrowException()
        {
            var expectedException = new Exception();
            _dapperMock.Setup(x => x.LoadDataAsync<Attendence>(It.IsAny<string>())).ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<ApplicationException>(_attendenceService.GetAllAsync);

            ClassicAssert.IsInstanceOf<ApplicationException>(exception);
        }

        [Test]
        public async Task GetByEmployeeIdAsync_Success()
        {
            IEnumerable<Attendence> testData =
            [
                new() { EmployeeId = 1, Year = 2015, Month = 5, TotalDaysWorked = 20, ExtraHoursWorked = 20 },
                new() { EmployeeId = 1, Year = 2015, Month = 6, TotalDaysWorked = 10, ExtraHoursWorked = 10 },
                new() { EmployeeId = 2, Year = 2015, Month = 5, TotalDaysWorked = 0, ExtraHoursWorked = 0 }
            ];

            _dapperMock.Setup(x => x.LoadDataWithParametersAsync<Attendence>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(testData);

            var result = await _attendenceService.GetByEmployeeIdAsync(1);

            ClassicAssert.IsNotNull(result);
            CollectionAssert.AreEqual(testData, result);
        }

        [Test]
        public void GetByEmployeeIdAsync_ShouldThrowException()
        {
            int invalidId = -1;
            var expectedException = new Exception();

            _dapperMock.Setup(x => x.LoadDataWithParametersAsync<Attendence>(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<ApplicationException>(
                async () => await _attendenceService.GetByEmployeeIdAsync(invalidId));

            ClassicAssert.IsInstanceOf<ApplicationException>(exception);
        }

        [Test]
        public async Task AddAsync_Success()
        {
            Attendence testData = new()
            {
                EmployeeId = 1,
                Year = 2015,
                Month = 5,
                TotalDaysWorked = 20,
                ExtraHoursWorked = 20
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(true);

            var result = await _attendenceService.AddAsync(testData);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task AddAsync_Fail()
        {
            Attendence testData = new()
            {
                EmployeeId = -1,
                Year = 2015,
                Month = 5,
                TotalDaysWorked = 20,
                ExtraHoursWorked = 20
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(false);

            var result = await _attendenceService.AddAsync(testData);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void AddAsync_ShouldThrowException()
        {
            Attendence testData = new()
            {
                EmployeeId = -1,
                Year = 2015,
                Month = 5,
                TotalDaysWorked = 20,
                ExtraHoursWorked = 20
            };
            var expectedException = new Exception();

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<ApplicationException>(
                async () => await _attendenceService.AddAsync(testData));

            ClassicAssert.IsInstanceOf<ApplicationException>(exception);
        }

        [Test]
        public async Task UpdateAsync_Success()
        {
            Attendence testData = new()
            {
                EmployeeId = 1,
                Year = 2015,
                Month = 5,
                TotalDaysWorked = 20,
                ExtraHoursWorked = 20
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(true);

            var result = await _attendenceService.UpdateAsync(testData);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAsync_Fail()
        {
            Attendence testData = new()
            {
                EmployeeId = 1,
                Year = 2015,
                Month = 5,
                TotalDaysWorked = 20,
                ExtraHoursWorked = 20
            };

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(false);

            var result = await _attendenceService.UpdateAsync(testData);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void UpdateAsync_ShouldThrowException()
        {
            Attendence testData = new()
            {
                EmployeeId = -1,
                Year = 2015,
                Month = 5,
                TotalDaysWorked = 20,
                ExtraHoursWorked = 20
            };
            var expectedException = new Exception();

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<ApplicationException>(
                async () => await _attendenceService.UpdateAsync(testData));

            ClassicAssert.IsInstanceOf<ApplicationException>(exception);
        }

        [Test]
        public async Task DeleteAsync_Success()
        {
            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(true);

            var result = await _attendenceService.DeleteAsync(1);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public async Task DeleteAsync_Fail()
        {
            int invalidId = -1;

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(false);

            var result = await _attendenceService.DeleteAsync(invalidId);

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsFalse(result);
        }

        [Test]
        public void DeleteAsync_ShouldThrowException()
        {
            int invalidId = -1;
            var expectedException = new Exception();

            _dapperMock.Setup(x => x.ExecuteSqlWithParametersAsync(
                It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ThrowsAsync(expectedException);

            var exception = Assert.ThrowsAsync<ApplicationException>(
                async () => await _attendenceService.DeleteAsync(invalidId));

            ClassicAssert.IsInstanceOf<ApplicationException>(exception);
        }
    }
}
