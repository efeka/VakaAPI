USE [master]
GO
/****** Object:  Database [VakaDB]    Script Date: 28.01.2024 19:52:35 ******/
CREATE DATABASE [VakaDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'VakaDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\VakaDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'VakaDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\VakaDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [VakaDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VakaDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VakaDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [VakaDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [VakaDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [VakaDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [VakaDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [VakaDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [VakaDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [VakaDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [VakaDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [VakaDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [VakaDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [VakaDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [VakaDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [VakaDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [VakaDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [VakaDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [VakaDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [VakaDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [VakaDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [VakaDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [VakaDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [VakaDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [VakaDB] SET RECOVERY FULL 
GO
ALTER DATABASE [VakaDB] SET  MULTI_USER 
GO
ALTER DATABASE [VakaDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [VakaDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [VakaDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [VakaDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [VakaDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [VakaDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'VakaDB', N'ON'
GO
ALTER DATABASE [VakaDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [VakaDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [VakaDB]
GO
/****** Object:  Schema [VakaSchema]    Script Date: 28.01.2024 19:52:35 ******/
CREATE SCHEMA [VakaSchema]
GO
/****** Object:  Table [VakaSchema].[Attendences]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [VakaSchema].[Attendences](
	[EmployeeId] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[TotalDaysWorked] [int] NULL,
	[ExtraHoursWorked] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC,
	[Year] ASC,
	[Month] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [VakaSchema].[Employees]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [VakaSchema].[Employees](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[TC] [varchar](11) NULL,
	[Name] [nvarchar](30) NULL,
	[Surname] [nvarchar](30) NULL,
	[EmployeeType] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [VakaSchema].[PaymentTypes]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [VakaSchema].[PaymentTypes](
	[PaymentId] [int] IDENTITY(1,1) NOT NULL,
	[PaymentType] [nvarchar](30) NULL,
	[Amount] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [VakaSchema].[Attendences]  WITH CHECK ADD FOREIGN KEY([EmployeeId])
REFERENCES [VakaSchema].[Employees] ([EmployeeId])
GO
ALTER TABLE [VakaSchema].[Attendences]  WITH CHECK ADD  CONSTRAINT [CHK_Month_Range] CHECK  (([Month]>=(1) AND [Month]<=(12)))
GO
ALTER TABLE [VakaSchema].[Attendences] CHECK CONSTRAINT [CHK_Month_Range]
GO
ALTER TABLE [VakaSchema].[Attendences]  WITH CHECK ADD  CONSTRAINT [CHK_Year_Range] CHECK  (([Year]>=(1) AND [Year]<=(9999)))
GO
ALTER TABLE [VakaSchema].[Attendences] CHECK CONSTRAINT [CHK_Year_Range]
GO
ALTER TABLE [VakaSchema].[Employees]  WITH CHECK ADD  CONSTRAINT [CHK_TC_Length] CHECK  ((len([TC])=(11)))
GO
ALTER TABLE [VakaSchema].[Employees] CHECK CONSTRAINT [CHK_TC_Length]
GO
ALTER TABLE [VakaSchema].[Employees]  WITH CHECK ADD  CONSTRAINT [CHK_Type_Range] CHECK  (([EmployeeType]>=(1) AND [EmployeeType]<=(3)))
GO
ALTER TABLE [VakaSchema].[Employees] CHECK CONSTRAINT [CHK_Type_Range]
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Attendences_DeleteByEmployeeId]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Attendences_DeleteByEmployeeId]
    @EmployeeId INT
AS BEGIN
    DELETE FROM VakaSchema.Attendences
    WHERE EmployeeId = @EmployeeId
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Attendences_GetAll]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Attendences_GetAll]
AS BEGIN
    SELECT *
    FROM VakaSchema.Attendences
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Attendences_GetByEmployeeId]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Attendences_GetByEmployeeId]
    @EmployeeId INT
AS BEGIN
    SELECT *
    FROM VakaSchema.Attendences
    WHERE EmployeeId = @EmployeeId
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Attendences_Insert]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Attendences_Insert]
    @EmployeeId INT,
    @Year INT,
    @Month INT,
    @TotalDaysWorked INT = 0,
	@ExtraHoursWorked INT = 0
AS BEGIN
    INSERT INTO VakaSchema.Attendences (
        EmployeeId,
        Year,
        Month,
        TotalDaysWorked,
        ExtraHoursWorked
    ) VALUES (
        @EmployeeId,
        @Year,
        @Month,
        @TotalDaysWorked,
        @ExtraHoursWorked
    )
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Attendences_UpdateEmployeeAttendence]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Attendences_UpdateEmployeeAttendence]
    @EmployeeId INT,
    @Year INT,
    @Month INT,
    @TotalDaysWorked INT,
	@ExtraHoursWorked INT
AS BEGIN
    UPDATE VakaSchema.Attendences
    SET TotalDaysWorked = @TotalDaysWorked,
        ExtraHoursWorked = @ExtraHoursWorked
    WHERE 
        EmployeeId = @EmployeeId AND
        Year = @Year AND
        Month = @Month
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Employees_Delete]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Employees_Delete]
    @EmployeeId INT
AS BEGIN
    DELETE FROM Employees
    WHERE EmployeeId = @EmployeeId
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Employees_GetAll]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Employees_GetAll]
AS BEGIN
    SELECT *
    FROM VakaSchema.Employees
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Employees_GetById]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Employees_GetById]
    @EmployeeId INT
AS BEGIN
    SELECT *
    FROM VakaSchema.Employees
    WHERE EmployeeId = @EmployeeId
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Employees_GetWithSalariesByDate]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Employees_GetWithSalariesByDate]
    @Year INT,
    @Month INT
AS BEGIN
    SELECT
        e.*,
        DATEFROMPARTS(@Year, @Month, 1) AS SalaryDate,
        COALESCE(
            CASE
                WHEN e.EmployeeType = 1 THEN pt_fixed.Amount
                WHEN e.EmployeeType = 2 THEN COALESCE(a.TotalDaysWorked, 0) * pt_daily.Amount
                WHEN e.EmployeeType = 3 THEN pt_fixed.Amount + COALESCE(a.ExtraHoursWorked, 0) * pt_hourly.Amount
            END
        , 0) AS Salary
    FROM 
        VakaSchema.Employees e
        LEFT JOIN VakaSchema.Attendences a ON e.EmployeeId = a.EmployeeId AND a.Year = @Year AND a.Month = @Month
        LEFT JOIN VakaSchema.PaymentTypes pt_fixed ON e.EmployeeType IN (1, 3) AND pt_fixed.PaymentType = 'Fixed Salary'
        LEFT JOIN VakaSchema.PaymentTypes pt_daily ON e.EmployeeType = 2 AND pt_daily.PaymentType = 'Daily Rate'
        LEFT JOIN VakaSchema.PaymentTypes pt_hourly ON e.EmployeeType = 3 AND pt_hourly.PaymentType = 'Overtime Rate'
    WHERE a.TotalDaysWorked != 0
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Employees_GetWithSalariesById]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Employees_GetWithSalariesById]
    @EmployeeId INT
AS BEGIN
    SELECT
        e.*,
        DATEFROMPARTS(a.[Year], a.[Month], 1) AS SalaryDate,
        COALESCE(
            CASE
                WHEN e.EmployeeType = 1 THEN pt_fixed.Amount
                WHEN e.EmployeeType = 2 THEN COALESCE(a.TotalDaysWorked, 0) * pt_daily.Amount
                WHEN e.EmployeeType = 3 THEN pt_fixed.Amount + COALESCE(a.ExtraHoursWorked, 0) * pt_hourly.Amount
            END
        , 0) AS Salary
    FROM 
        VakaSchema.Employees e
        LEFT JOIN VakaSchema.Attendences a ON e.EmployeeId = a.EmployeeId
        LEFT JOIN VakaSchema.PaymentTypes pt_fixed ON e.EmployeeType IN (1, 3) AND pt_fixed.PaymentType = 'Fixed Salary'
        LEFT JOIN VakaSchema.PaymentTypes pt_daily ON e.EmployeeType = 2 AND pt_daily.PaymentType = 'Daily Rate'
        LEFT JOIN VakaSchema.PaymentTypes pt_hourly ON e.EmployeeType = 3 AND pt_hourly.PaymentType = 'Overtime Rate'
    WHERE e.EmployeeId = @EmployeeId AND a.TotalDaysWorked != 0
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Employees_Insert]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Employees_Insert]
    @TC VARCHAR(11),
    @Name NVARCHAR(30),
    @Surname NVARCHAR(30),
    @EmployeeType INT
AS BEGIN
    INSERT INTO VakaSchema.Employees (
        TC,
        Name,
        Surname,
        EmployeeType
    ) VALUES (
        @TC,
        @Name,
        @Surname,
        @EmployeeType
    )
END;
GO
/****** Object:  StoredProcedure [VakaSchema].[sp_Employees_Update]    Script Date: 28.01.2024 19:52:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [VakaSchema].[sp_Employees_Update]
    @EmployeeId INT,
    @TC VARCHAR(11),
    @Name NVARCHAR(30),
    @Surname NVARCHAR(30),
    @EmployeeType INT
AS BEGIN
    UPDATE VakaSchema.Employees
    SET TC = @TC,
        Name = @Name,
        Surname = @Surname,
        EmployeeType = @EmployeeType
    WHERE EmployeeId = @EmployeeId
END;
GO

INSERT INTO VakaSchema.PaymentTypes
VALUES ('Fixed Salary', 9000);

INSERT INTO VakaSchema.PaymentTypes
VALUES ('Daily Rate', 300);

INSERT INTO VakaSchema.PaymentTypes
VALUES ('Overtime Rate', 30);

USE [master]
GO
ALTER DATABASE [VakaDB] SET  READ_WRITE 
GO
