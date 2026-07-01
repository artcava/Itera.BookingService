/****** Object:  Table [dbo].[Km]    Script Date: 30/06/2026 15:05:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Km](
	[kmID] [int] IDENTITY(1,1) NOT NULL,
	[DurataID] [char](1) NOT NULL,
	[CodiceCategoria] [varchar](5) NOT NULL,
	[Giorni] [int] NOT NULL,
	[Inizio] [int] NULL,
	[Fine] [int] NULL,
	[Descrizione] [nvarchar](100) NOT NULL,
	[Ordinamento] [int] NOT NULL,
 CONSTRAINT [PK_Km] PRIMARY KEY CLUSTERED 
(
	[kmID] ASC
)WITH (STATISTICS_NORECOMPUTE = ON, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Km] ADD  CONSTRAINT [DF_Km_Ordine]  DEFAULT ((0)) FOR [Ordinamento]
GO

ALTER TABLE [dbo].[Km]  WITH CHECK ADD  CONSTRAINT [FK_Km_CategoriaSegmento] FOREIGN KEY([CodiceCategoria])
REFERENCES [dbo].[CategoriaSegmento] ([CodiceCategoria])
GO

ALTER TABLE [dbo].[Km] CHECK CONSTRAINT [FK_Km_CategoriaSegmento]
GO

ALTER TABLE [dbo].[Km]  WITH CHECK ADD  CONSTRAINT [FK_Km_Durata] FOREIGN KEY([DurataID])
REFERENCES [dbo].[Durata] ([DurataID])
GO

ALTER TABLE [dbo].[Km] CHECK CONSTRAINT [FK_Km_Durata]
GO

