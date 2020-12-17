USE [EcoliDecoy]
GO
/****** Object:  Table [dbo].[ProteinInfoes]    Script Date: 17-Dec-20 12:28:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProteinInfoes](
	[ID] [nvarchar](255) NOT NULL,
	[MW] [float] NOT NULL,
	[Seq] [nvarchar](max) NOT NULL,
	[Insilico] [nvarchar](max) NOT NULL,
	[InsilicoR] [nvarchar](max) NOT NULL,
	[FastaHeader] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
