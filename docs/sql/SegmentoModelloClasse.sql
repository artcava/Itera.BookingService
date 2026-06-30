/****** Object:  Table [dbo].[SegmentoModelloClasse]    Script Date: 30/06/2026 09:42:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SegmentoModelloClasse](
	[SegmentoModelloClasseID] [int] NOT NULL,
	[Descrizione] [nvarchar](50) NULL,
	[DataInserimento] [datetime] NULL,
	[DataUltimaModifica] [datetime] NULL,
	[LeadDays] [int] NULL,
 CONSTRAINT [PK_SegmentoModelloClasse] PRIMARY KEY CLUSTERED 
(
	[SegmentoModelloClasseID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

