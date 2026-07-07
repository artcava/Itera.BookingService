/****** Object:  Table [dbo].[TipologiaVoceFattura]    Script Date: 07/07/2026 09:30:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TipologiaVoceFattura](
	[TipologiaVoceFatturaID] [tinyint] NOT NULL,
	[Descrizione] [nvarchar](250) NOT NULL,
	[DescrizioneFatturazione] [varchar](250) NULL,
	[CodArticoloFatturazione] [varchar](10) NULL,
	[IsNotaCredito] [bit] NOT NULL,
	[CategoriaFatturaID] [tinyint] NULL,
	[CodiceCategoriaSegmento] [varchar](5) NULL,
	[RipartizioneFatturatoID] [char](1) NULL,
	[Attiva] [bit] NOT NULL,
	[CodArticoloFatturazioneSostitutivo] [varchar](10) NULL,
	[DataInserimento] [datetime] NULL,
	[DataUltimaModifica] [datetime] NULL,
	[FatturazioneSuProprietaFisicaMezzo] [bit] NULL,
	[IvaID] [smallint] NULL,
 CONSTRAINT [PK_TipologiaVoceFattura] PRIMARY KEY CLUSTERED 
(
	[TipologiaVoceFatturaID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TipologiaVoceFattura] ADD  CONSTRAINT [DF_TipologiaVoceFattura_IsNotaCredito]  DEFAULT ((0)) FOR [IsNotaCredito]
GO

ALTER TABLE [dbo].[TipologiaVoceFattura]  WITH CHECK ADD  CONSTRAINT [FK_TipologiaVoceFattura_CategoriaFattura] FOREIGN KEY([CategoriaFatturaID])
REFERENCES [dbo].[CategoriaFattura] ([CategoriaFatturaID])
GO

ALTER TABLE [dbo].[TipologiaVoceFattura] CHECK CONSTRAINT [FK_TipologiaVoceFattura_CategoriaFattura]
GO

ALTER TABLE [dbo].[TipologiaVoceFattura]  WITH CHECK ADD  CONSTRAINT [FK_TipologiaVoceFattura_CategoriaSegmento] FOREIGN KEY([CodiceCategoriaSegmento])
REFERENCES [dbo].[CategoriaSegmento] ([CodiceCategoria])
GO

ALTER TABLE [dbo].[TipologiaVoceFattura] CHECK CONSTRAINT [FK_TipologiaVoceFattura_CategoriaSegmento]
GO

ALTER TABLE [dbo].[TipologiaVoceFattura]  WITH NOCHECK ADD  CONSTRAINT [FK_TipologiaVoceFattura_Iva] FOREIGN KEY([IvaID])
REFERENCES [dbo].[Iva] ([IvaID])
GO

ALTER TABLE [dbo].[TipologiaVoceFattura] CHECK CONSTRAINT [FK_TipologiaVoceFattura_Iva]
GO

ALTER TABLE [dbo].[TipologiaVoceFattura]  WITH CHECK ADD  CONSTRAINT [FK_TipologiaVoceFattura_RipartizioneFatturatoID] FOREIGN KEY([RipartizioneFatturatoID])
REFERENCES [dbo].[RipartizioneFatturato] ([RipartizioneFatturatoID])
GO

ALTER TABLE [dbo].[TipologiaVoceFattura] CHECK CONSTRAINT [FK_TipologiaVoceFattura_RipartizioneFatturatoID]
GO

