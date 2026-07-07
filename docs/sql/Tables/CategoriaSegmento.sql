/****** Object:  Table [dbo].[CategoriaSegmento]    Script Date: 07/07/2026 09:41:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CategoriaSegmento](
	[CodiceCategoria] [varchar](5) NOT NULL,
	[Descrizione] [nvarchar](50) NOT NULL,
	[VoceContabile] [nvarchar](10) NOT NULL,
	[DescrizioneVoceContabile] [nvarchar](50) NOT NULL,
	[NomeFileImmagine] [varchar](100) NOT NULL,
	[Tipo] [char](1) NOT NULL,
	[DescrizioneSAP] [varchar](15) NULL,
 CONSTRAINT [PK_CategoriaSegmento] PRIMARY KEY CLUSTERED 
(
	[CodiceCategoria] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

