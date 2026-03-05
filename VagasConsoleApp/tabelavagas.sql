USE [suabasededados]
GO

/****** Object:  Table [dbo].[Vagas]    Script Date: 04/03/2026 23:12:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Vagas](
	[Id] [int] NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Company] [nvarchar](200) NULL,
	[City] [nvarchar](100) NULL,
	[Salary] [decimal](18, 2) NULL,
	[Requirements] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[site] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


