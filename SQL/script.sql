/****** Object:  Database [energysavings]    Script Date: 04-12-2021 09:56:59 ******/
CREATE DATABASE [energysavings]  (EDITION = 'Standard', SERVICE_OBJECTIVE = 'S0', MAXSIZE = 250 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS;
GO
ALTER DATABASE [energysavings] SET COMPATIBILITY_LEVEL = 150
GO
ALTER DATABASE [energysavings] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [energysavings] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [energysavings] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [energysavings] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [energysavings] SET ARITHABORT OFF 
GO
ALTER DATABASE [energysavings] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [energysavings] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [energysavings] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [energysavings] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [energysavings] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [energysavings] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [energysavings] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [energysavings] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [energysavings] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [energysavings] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [energysavings] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [energysavings] SET  MULTI_USER 
GO
ALTER DATABASE [energysavings] SET ENCRYPTION ON
GO
ALTER DATABASE [energysavings] SET QUERY_STORE = ON
GO
ALTER DATABASE [energysavings] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[cts_rawdata]    Script Date: 04-12-2021 09:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cts_rawdata](
	[receivedtime] [datetime] NOT NULL,
	[rawdata] [nvarchar](max) NULL,
	[IsProcessed] [smallint] NULL,
 CONSTRAINT [PK_cts_rawdata] PRIMARY KEY CLUSTERED 
(
	[receivedtime] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbldevices]    Script Date: 04-12-2021 09:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbldevices](
	[DeviceId] [bigint] IDENTITY(1,1) NOT NULL,
	[LocationId] [int] NULL,
	[DeviceTypeId] [smallint] NULL,
	[reportedAt] [datetime] NULL,
	[Occupancy] [smallint] NULL,
 CONSTRAINT [PK_tbldevices] PRIMARY KEY CLUSTERED 
(
	[DeviceId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblenergysaving]    Script Date: 04-12-2021 09:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblenergysaving](
	[rawdataid] [bigint] IDENTITY(1,1) NOT NULL,
	[locationId] [varchar](50) NULL,
	[deviceId] [varchar](50) NULL,
	[reportedAt] [datetime] NULL,
	[Power] [float] NULL,
	[Temperature] [float] NULL,
	[Humidity] [float] NULL,
	[Occupancy] [int] NULL,
	[ArmState] [varchar](50) NULL,
 CONSTRAINT [PK_tblenergysaving] PRIMARY KEY CLUSTERED 
(
	[rawdataid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbltrainingData]    Script Date: 04-12-2021 09:56:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbltrainingData](
	[Traindataid] [bigint] IDENTITY(1,1) NOT NULL,
	[Locationid] [varchar](50) NULL,
	[Zoneid] [varchar](50) NULL,
	[ReportedAt] [datetime] NULL,
	[Power] [float] NULL,
	[Temperature] [float] NULL,
	[Humidity] [float] NULL,
	[Occupancy] [int] NULL,
	[ArmState] [varchar](50) NULL,
	[IsFan] [int] NULL,
	[IsLight] [int] NULL,
	[IsTV] [int] NULL,
	[IsWashingMachine] [int] NULL,
	[IsAC] [int] NULL,
	[Energysaving] [int] NULL,
	[IsOtherDevices] [int] NULL,
 CONSTRAINT [PK_tbltrainingData] PRIMARY KEY CLUSTERED 
(
	[Traindataid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'F=1,L=2,T=3,W=4,AC=5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbldevices', @level2type=N'COLUMN',@level2name=N'DeviceTypeId'
GO
ALTER DATABASE [energysavings] SET  READ_WRITE 
GO
