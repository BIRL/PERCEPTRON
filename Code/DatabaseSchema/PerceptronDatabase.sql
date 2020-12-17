USE [PerceptronDatabase]
GO
/****** Object:  Table [dbo].[ExecutionTimes]    Script Date: 17-Dec-20 12:30:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExecutionTimes](
	[InsilicoTime] [nvarchar](max) NULL,
	[PtmTime] [nvarchar](max) NULL,
	[TunerTime] [nvarchar](max) NULL,
	[MwFilterTime] [nvarchar](max) NULL,
	[PstTime] [nvarchar](max) NULL,
	[TotalTime] [nvarchar](max) NULL,
	[QueryId] [nvarchar](128) NOT NULL,
	[FileName] [nvarchar](128) NOT NULL,
	[TruncationEngineTime] [nvarchar](max) NULL,
 CONSTRAINT [PK_ExecutionTimes_1] PRIMARY KEY CLUSTERED 
(
	[QueryId] ASC,
	[FileName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PeakListData]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PeakListData](
	[FileUniqueId] [nvarchar](128) NOT NULL,
	[PeakListMasses] [nvarchar](max) NULL,
	[PeakListIntensities] [nvarchar](max) NULL,
 CONSTRAINT [PK_PeakListData] PRIMARY KEY CLUSTERED 
(
	[FileUniqueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PtmFixedModifications]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PtmFixedModifications](
	[ModificationId] [int] IDENTITY(1,1) NOT NULL,
	[QueryId] [nvarchar](128) NULL,
	[FixedModifications] [nvarchar](max) NULL,
 CONSTRAINT [PK_PtmFixedModifications] PRIMARY KEY CLUSTERED 
(
	[ModificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PtmVariableModifications]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PtmVariableModifications](
	[ModificationId] [int] IDENTITY(1,1) NOT NULL,
	[QueryId] [nvarchar](128) NULL,
	[VariableModifications] [nvarchar](max) NULL,
 CONSTRAINT [PK_PtmVariableModifications_1] PRIMARY KEY CLUSTERED 
(
	[ModificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResultInsilicoMatchLefts]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResultInsilicoMatchLefts](
	[ResultId] [nvarchar](max) NULL,
	[MatchedPeaks] [nvarchar](max) NULL,
	[RowId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_dbo.ResultInsilicoMatchLefts] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResultInsilicoMatchRights]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResultInsilicoMatchRights](
	[ResultId] [nvarchar](max) NULL,
	[MatchedPeaks] [nvarchar](max) NULL,
	[RowId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_dbo.ResultInsilicoMatchRights] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResultPtmSites]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResultPtmSites](
	[ResultPtmSitesId] [int] IDENTITY(1,1) NOT NULL,
	[ResultId] [nvarchar](128) NOT NULL,
	[Index] [nvarchar](max) NOT NULL,
	[ModName] [nvarchar](max) NOT NULL,
	[Site] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ResultPtmSites_1] PRIMARY KEY CLUSTERED 
(
	[ResultPtmSitesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResultsDownloadable]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResultsDownloadable](
	[QueryId] [nvarchar](128) NOT NULL,
	[ResultId] [nvarchar](128) NOT NULL,
	[MassSpectrumImageFilePath] [nvarchar](max) NOT NULL,
	[JsonExpThrTableFilePath] [nvarchar](max) NOT NULL,
	[CompleteDetailedResultsFilePath] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ResultsDownloadable] PRIMARY KEY CLUSTERED 
(
	[ResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SearchFiles]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchFiles](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[QueryId] [nvarchar](max) NULL,
	[FileName] [nvarchar](max) NULL,
	[FileType] [nvarchar](max) NULL,
	[UniqueFileName] [nvarchar](max) NULL,
	[FileUniqueId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.SearchFiles] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SearchParameters]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchParameters](
	[QueryId] [nvarchar](128) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[EmailId] [nvarchar](max) NULL,
	[UserId] [nvarchar](max) NOT NULL,
	[ProteinDatabase] [nvarchar](max) NOT NULL,
	[NumberOfOutputs] [nvarchar](max) NOT NULL,
	[MassMode] [nvarchar](max) NULL,
	[FilterDb] [nvarchar](max) NOT NULL,
	[MwTolerance] [float] NOT NULL,
	[MwToleranceUnit] [nvarchar](max) NULL,
	[PeptideTolerance] [float] NOT NULL,
	[PeptideToleranceUnit] [nvarchar](max) NOT NULL,
	[Autotune] [nvarchar](max) NOT NULL,
	[NeutralLoss] [float] NOT NULL,
	[SliderValue] [float] NOT NULL,
	[InsilicoFragType] [nvarchar](max) NULL,
	[HandleIons] [nvarchar](max) NULL,
	[DenovoAllow] [nvarchar](max) NOT NULL,
	[MinimumPstLength] [int] NOT NULL,
	[MaximumPstLength] [int] NOT NULL,
	[HopThreshhold] [float] NOT NULL,
	[HopTolUnit] [nvarchar](max) NULL,
	[PSTTolerance] [float] NOT NULL,
	[Truncation] [nvarchar](max) NOT NULL,
	[TerminalModification] [nvarchar](max) NOT NULL,
	[PtmAllow] [nvarchar](max) NOT NULL,
	[PtmTolerance] [float] NOT NULL,
	[PtmToleranceUnit] [nvarchar](max) NULL,
	[MethionineChemicalModification] [nvarchar](max) NOT NULL,
	[CysteineChemicalModification] [nvarchar](max) NOT NULL,
	[MwSweight] [float] NOT NULL,
	[PstSweight] [float] NOT NULL,
	[InsilicoSweight] [float] NOT NULL,
	[TruncationIndex] [int] NOT NULL,
	[FDRCutOff] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SearchParameters] PRIMARY KEY CLUSTERED 
(
	[QueryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SearchQueries]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchQueries](
	[QueryId] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](max) NOT NULL,
	[Progress] [nvarchar](max) NULL,
	[CreationTime] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SearchQueries] PRIMARY KEY CLUSTERED 
(
	[QueryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SearchResults]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchResults](
	[QueryId] [nvarchar](128) NOT NULL,
	[ResultId] [nvarchar](128) NOT NULL,
	[Header] [nvarchar](max) NULL,
	[Sequence] [nvarchar](max) NULL,
	[PstScore] [float] NOT NULL,
	[InsilicoScore] [float] NOT NULL,
	[PtmScore] [float] NOT NULL,
	[Score] [float] NOT NULL,
	[MwScore] [float] NOT NULL,
	[Mw] [float] NOT NULL,
	[FileId] [nvarchar](max) NULL,
	[OriginalSequence] [nvarchar](max) NULL,
	[PSTTags] [nvarchar](max) NULL,
	[RightMatchedIndex] [nvarchar](max) NULL,
	[RightPeakIndex] [nvarchar](max) NULL,
	[RightType] [nvarchar](max) NULL,
	[LeftMatchedIndex] [nvarchar](max) NULL,
	[LeftPeakIndex] [nvarchar](max) NULL,
	[LeftType] [nvarchar](max) NULL,
	[InsilicoMassLeft] [nvarchar](max) NULL,
	[InsilicoMassRight] [nvarchar](max) NULL,
	[InsilicoMassLeftAo] [nvarchar](max) NULL,
	[InsilicoMassLeftBo] [nvarchar](max) NULL,
	[InsilicoMassLeftAstar] [nvarchar](max) NULL,
	[InsilicoMassLeftBstar] [nvarchar](max) NULL,
	[InsilicoMassRightYo] [nvarchar](max) NULL,
	[InsilicoMassRightYstar] [nvarchar](max) NULL,
	[InsilicoMassRightZo] [nvarchar](max) NULL,
	[InsilicoMassRightZoo] [nvarchar](max) NULL,
	[TerminalModification] [nvarchar](max) NULL,
	[TruncationSite] [nvarchar](max) NULL,
	[TruncationIndex] [int] NOT NULL,
	[FileUniqueId] [nvarchar](128) NOT NULL,
	[Evalue] [float] NOT NULL,
	[BlindPtmLocalization] [nvarchar](max) NOT NULL,
	[ProteinRank] [int] NOT NULL,
 CONSTRAINT [PK_SearchResults] PRIMARY KEY CLUSTERED 
(
	[QueryId] ASC,
	[ResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Test]    Script Date: 17-Dec-20 12:30:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Test](
	[QueryId] [nvarchar](128) NOT NULL,
	[Path] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ExecutionTimes] ADD  CONSTRAINT [DF__Execution__Query__36B12243]  DEFAULT ('') FOR [QueryId]
GO
ALTER TABLE [dbo].[ResultPtmSites] ADD  CONSTRAINT [DF__ResultPtmS__Site__77AABCF8]  DEFAULT ('XYZ') FOR [Site]
GO
ALTER TABLE [dbo].[SearchFiles] ADD  CONSTRAINT [DF__SearchFil__FileU__1E05700A]  DEFAULT ('UniqueID-XYZ') FOR [FileUniqueId]
GO
ALTER TABLE [dbo].[SearchParameters] ADD  CONSTRAINT [DF__SearchPar__Pepti__70A8B9AE]  DEFAULT ('ppm') FOR [PeptideToleranceUnit]
GO
ALTER TABLE [dbo].[SearchParameters] ADD  CONSTRAINT [DF_SearchParameters_NeutralLoss]  DEFAULT ((0.0)) FOR [NeutralLoss]
GO
ALTER TABLE [dbo].[SearchParameters] ADD  CONSTRAINT [DF_SearchParameters_SliderValue]  DEFAULT ((50.0)) FOR [SliderValue]
GO
ALTER TABLE [dbo].[SearchParameters] ADD  CONSTRAINT [DF__SearchPar__PSTTo__531856C7]  DEFAULT ((0.5)) FOR [PSTTolerance]
GO
ALTER TABLE [dbo].[SearchParameters] ADD  CONSTRAINT [DF_SearchParameters_TerminalModification]  DEFAULT (N'None') FOR [TerminalModification]
GO
ALTER TABLE [dbo].[SearchParameters] ADD  CONSTRAINT [DF__SearchPar__Methi__10E07F16]  DEFAULT ('None') FOR [MethionineChemicalModification]
GO
ALTER TABLE [dbo].[SearchParameters] ADD  CONSTRAINT [DF__SearchPar__Cyste__0FEC5ADD]  DEFAULT ('None') FOR [CysteineChemicalModification]
GO
ALTER TABLE [dbo].[SearchParameters] ADD  CONSTRAINT [DF__SearchPar__Trunc__1A34DF26]  DEFAULT ((-1000)) FOR [TruncationIndex]
GO
ALTER TABLE [dbo].[SearchResults] ADD  CONSTRAINT [DF__SearchRes__Trunc__1C1D2798]  DEFAULT ((-1000)) FOR [TruncationIndex]
GO
ALTER TABLE [dbo].[SearchResults] ADD  CONSTRAINT [DF__SearchRes__FileU__1FEDB87C]  DEFAULT ('NO ID') FOR [FileUniqueId]
GO
ALTER TABLE [dbo].[SearchResults] ADD  CONSTRAINT [DF__SearchRes__Evalu__72E607DB]  DEFAULT ((0.0)) FOR [Evalue]
GO
ALTER TABLE [dbo].[SearchResults] ADD  CONSTRAINT [DF__SearchRes__Blind__73DA2C14]  DEFAULT ('-1,-1,-1') FOR [BlindPtmLocalization]
GO
ALTER TABLE [dbo].[SearchResults] ADD  DEFAULT ((0)) FOR [ProteinRank]
GO
