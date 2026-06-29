/****** Object:  Table [dbo].[SegmentoModello]    Script Date: 29/06/2026 17:12:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SegmentoModello](
	[CodiceSegmento] [varchar](3) NOT NULL,
	[CodiceCategoria] [varchar](5) NOT NULL,
	[Descrizione] [nvarchar](50) NOT NULL,
	[Ordinamento] [tinyint] NOT NULL,
	[ModelloMezzoIDPdfOfferta] [int] NULL,
	[Stato] [tinyint] NULL,
	[ModelloMezzoIDErs] [int] NULL,
	[FleetID] [char](1) NULL,
	[SegmentoModelloClasseID] [int] NULL,
	[IndexPricing] [smallint] NULL,
	[ListinoID] [int] NULL,
	[ImportoVAL] [money] NULL,
	[DataInserimento] [datetime] NULL,
	[DataUltimaModifica] [datetime] NULL,
 CONSTRAINT [PK_SegmentoModello] PRIMARY KEY CLUSTERED 
(
	[CodiceSegmento] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SegmentoModello] ADD  CONSTRAINT [DF_SegmentoModello_Ordinamento]  DEFAULT ((0)) FOR [Ordinamento]
GO

ALTER TABLE [dbo].[SegmentoModello] ADD  CONSTRAINT [DF_SegmentoModello_Stato]  DEFAULT ((1)) FOR [Stato]
GO

ALTER TABLE [dbo].[SegmentoModello]  WITH CHECK ADD  CONSTRAINT [FK_SegmentoModello_CategoriaSegmento1] FOREIGN KEY([CodiceCategoria])
REFERENCES [dbo].[CategoriaSegmento] ([CodiceCategoria])
GO

ALTER TABLE [dbo].[SegmentoModello] CHECK CONSTRAINT [FK_SegmentoModello_CategoriaSegmento1]
GO

ALTER TABLE [dbo].[SegmentoModello]  WITH CHECK ADD  CONSTRAINT [FK_SegmentoModello_Fleet] FOREIGN KEY([FleetID])
REFERENCES [dbo].[Fleet] ([FleetID])
GO

ALTER TABLE [dbo].[SegmentoModello] CHECK CONSTRAINT [FK_SegmentoModello_Fleet]
GO

ALTER TABLE [dbo].[SegmentoModello]  WITH CHECK ADD  CONSTRAINT [FK_SegmentoModello_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[SegmentoModello] CHECK CONSTRAINT [FK_SegmentoModello_Listino]
GO

ALTER TABLE [dbo].[SegmentoModello]  WITH CHECK ADD  CONSTRAINT [FK_SegmentoModello_ModelloMezzo] FOREIGN KEY([ModelloMezzoIDPdfOfferta])
REFERENCES [dbo].[ModelloMezzo] ([ModelloMezzoID])
GO

ALTER TABLE [dbo].[SegmentoModello] CHECK CONSTRAINT [FK_SegmentoModello_ModelloMezzo]
GO

ALTER TABLE [dbo].[SegmentoModello]  WITH CHECK ADD  CONSTRAINT [FK_SegmentoModello_ModelloMezzo1] FOREIGN KEY([ModelloMezzoIDErs])
REFERENCES [dbo].[ModelloMezzo] ([ModelloMezzoID])
GO

ALTER TABLE [dbo].[SegmentoModello] CHECK CONSTRAINT [FK_SegmentoModello_ModelloMezzo1]
GO

ALTER TABLE [dbo].[SegmentoModello]  WITH CHECK ADD  CONSTRAINT [FK_SegmentoModello_SegmentoModelloClasse] FOREIGN KEY([SegmentoModelloClasseID])
REFERENCES [dbo].[SegmentoModelloClasse] ([SegmentoModelloClasseID])
GO

ALTER TABLE [dbo].[SegmentoModello] CHECK CONSTRAINT [FK_SegmentoModello_SegmentoModelloClasse]
GO

