/****** Object:  Table [dbo].[ListinoFranchigia]    Script Date: 30/06/2026 14:14:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ListinoFranchigia](
	[ListinoFranchigiaID] [int] IDENTITY(1,1) NOT NULL,
	[DurataID] [char](1) NOT NULL,
	[CodiceSegmento] [varchar](3) NOT NULL,
	[PenaleRisarcitoriaRCAuto] [money] NOT NULL,
	[PenaleRisarcitoriaDanni] [money] NOT NULL,
	[PenaleRisarcitoriaIncendioFurto] [money] NOT NULL,
	[CostoCoperturaExtra] [money] NOT NULL,
	[CostoCoperturaExtraSoglia] [money] NOT NULL,
	[PenaleRisarcitoriaDanniRidotta] [money] NOT NULL,
	[PenaleRisarcitoriaIncendioFurtoRidotta] [money] NOT NULL,
	[ValidaDal] [datetime] NOT NULL,
	[ValidaAl] [datetime] NULL,
	[ListinoID] [int] NOT NULL,
	[TipologiaFranchigiaID] [nvarchar](3) NOT NULL,
	[DataUltimaModifica] [datetime] NULL,
	[DataInserimento] [datetime] NULL,
	[SubCodice] [varchar](50) NULL,
	[BreakEven] [smallint] NULL,
	[MinGiorniApplicabilita] [smallint] NULL,
	[MaxGiorniApplicabilita] [smallint] NULL,
	[TipoImporto] [varchar](3) NULL,
	[ImportoGiornoExtra] [money] NULL,
 CONSTRAINT [PK_ListinoFranchigia] PRIMARY KEY CLUSTERED 
(
	[ListinoFranchigiaID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ListinoFranchigia]  WITH CHECK ADD  CONSTRAINT [FK_ListinoFranchigia_Durata] FOREIGN KEY([DurataID])
REFERENCES [dbo].[Durata] ([DurataID])
GO

ALTER TABLE [dbo].[ListinoFranchigia] CHECK CONSTRAINT [FK_ListinoFranchigia_Durata]
GO

ALTER TABLE [dbo].[ListinoFranchigia]  WITH CHECK ADD  CONSTRAINT [FK_ListinoFranchigia_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[ListinoFranchigia] CHECK CONSTRAINT [FK_ListinoFranchigia_Listino]
GO

ALTER TABLE [dbo].[ListinoFranchigia]  WITH CHECK ADD  CONSTRAINT [FK_ListinoFranchigia_SegmentoModello] FOREIGN KEY([CodiceSegmento])
REFERENCES [dbo].[SegmentoModello] ([CodiceSegmento])
GO

ALTER TABLE [dbo].[ListinoFranchigia] CHECK CONSTRAINT [FK_ListinoFranchigia_SegmentoModello]
GO

ALTER TABLE [dbo].[ListinoFranchigia]  WITH CHECK ADD  CONSTRAINT [FK_ListinoFranchigia_TipologiaFranchigia] FOREIGN KEY([TipologiaFranchigiaID])
REFERENCES [dbo].[TipologiaFranchigia] ([TipologiaFranchigiaID])
GO

ALTER TABLE [dbo].[ListinoFranchigia] CHECK CONSTRAINT [FK_ListinoFranchigia_TipologiaFranchigia]
GO

