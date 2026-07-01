/****** Object:  Table [dbo].[Preventivo]    Script Date: 30/06/2026 14:11:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Preventivo](
	[PreventivoID] [int] IDENTITY(1,1) NOT NULL,
	[DataDocumento] [datetime] NOT NULL,
	[CodiceCategoria] [varchar](5) NOT NULL,
	[CodiceSegmento] [varchar](3) NOT NULL,
	[CodiceDurata] [char](1) NOT NULL,
	[KmID] [int] NOT NULL,
	[Giorni] [smallint] NOT NULL,
	[DataInizio] [datetime] NULL,
	[DataFine] [datetime] NULL,
	[ListinoID] [int] NOT NULL,
	[ListinoScontisticaID] [int] NULL,
	[AssicurazioneExtra] [bit] NOT NULL,
	[FilialeID] [int] NOT NULL,
	[OperatoreID] [int] NOT NULL,
	[Importo] [money] NOT NULL,
	[IvaID] [smallint] NOT NULL,
	[ContatoreCliente] [int] NOT NULL,
	[FilialeDestinazioneID] [int] NULL,
	[VAL] [bit] NULL,
	[VALImporto] [money] NULL,
 CONSTRAINT [PK_Preventivo] PRIMARY KEY CLUSTERED 
(
	[PreventivoID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Preventivo] ADD  CONSTRAINT [DF_Preventivo_ContatoreCliente]  DEFAULT ((0)) FOR [ContatoreCliente]
GO

ALTER TABLE [dbo].[Preventivo]  WITH CHECK ADD  CONSTRAINT [FK_Preventivo_CategoriaSegmento] FOREIGN KEY([CodiceCategoria])
REFERENCES [dbo].[CategoriaSegmento] ([CodiceCategoria])
GO

ALTER TABLE [dbo].[Preventivo] CHECK CONSTRAINT [FK_Preventivo_CategoriaSegmento]
GO

ALTER TABLE [dbo].[Preventivo]  WITH CHECK ADD  CONSTRAINT [FK_Preventivo_Durata] FOREIGN KEY([CodiceDurata])
REFERENCES [dbo].[Durata] ([DurataID])
GO

ALTER TABLE [dbo].[Preventivo] CHECK CONSTRAINT [FK_Preventivo_Durata]
GO

ALTER TABLE [dbo].[Preventivo]  WITH CHECK ADD  CONSTRAINT [FK_Preventivo_Filiale] FOREIGN KEY([FilialeID])
REFERENCES [dbo].[Filiale] ([FilialeID])
GO

ALTER TABLE [dbo].[Preventivo] CHECK CONSTRAINT [FK_Preventivo_Filiale]
GO

ALTER TABLE [dbo].[Preventivo]  WITH CHECK ADD  CONSTRAINT [FK_Preventivo_Iva] FOREIGN KEY([IvaID])
REFERENCES [dbo].[Iva] ([IvaID])
GO

ALTER TABLE [dbo].[Preventivo] CHECK CONSTRAINT [FK_Preventivo_Iva]
GO

ALTER TABLE [dbo].[Preventivo]  WITH CHECK ADD  CONSTRAINT [FK_Preventivo_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[Preventivo] CHECK CONSTRAINT [FK_Preventivo_Listino]
GO

ALTER TABLE [dbo].[Preventivo]  WITH CHECK ADD  CONSTRAINT [FK_Preventivo_Operatore] FOREIGN KEY([OperatoreID])
REFERENCES [dbo].[Operatore] ([OperatoreID])
GO

ALTER TABLE [dbo].[Preventivo] CHECK CONSTRAINT [FK_Preventivo_Operatore]
GO

ALTER TABLE [dbo].[Preventivo]  WITH CHECK ADD  CONSTRAINT [FK_Preventivo_SegmentoModello] FOREIGN KEY([CodiceSegmento])
REFERENCES [dbo].[SegmentoModello] ([CodiceSegmento])
GO

ALTER TABLE [dbo].[Preventivo] CHECK CONSTRAINT [FK_Preventivo_SegmentoModello]
GO

