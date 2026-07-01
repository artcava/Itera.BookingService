/****** Object:  Table [dbo].[AccordoCommercialeListino]    Script Date: 30/06/2026 14:15:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccordoCommercialeListino](
	[AccordoCommercialeListinoID] [int] IDENTITY(1,1) NOT NULL,
	[AccordoCommercialeID] [int] NOT NULL,
	[ListinoID] [int] NOT NULL,
	[PeriodoValiditaDa] [datetime] NOT NULL,
	[PeriodoValiditaA] [datetime] NOT NULL,
	[ProdottoEsclusivo] [bit] NOT NULL,
	[DataInserimento] [datetime] NULL,
	[DataUltimaModifica] [datetime] NULL,
	[TariffarioID] [int] NOT NULL,
	[ScontoPenaleRisarcitoriaFurto] [decimal](5, 2) NULL,
	[ScontoPenaleRisarcitoriaDanni] [decimal](5, 2) NULL,
	[ScontoPenaleRisarcitoriaRidottaFurto] [decimal](5, 2) NULL,
	[ScontoPenaleRisarcitoriaRidottaDanni] [decimal](5, 2) NULL,
 CONSTRAINT [PK_AccordoCommercialeListino] PRIMARY KEY CLUSTERED 
(
	[AccordoCommercialeListinoID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AccordoCommercialeListino]  WITH NOCHECK ADD  CONSTRAINT [FK_AccordoCommercialeListino_AccordoCommerciale] FOREIGN KEY([AccordoCommercialeID])
REFERENCES [dbo].[AccordoCommerciale] ([AccordoCommercialeID])
GO

ALTER TABLE [dbo].[AccordoCommercialeListino] CHECK CONSTRAINT [FK_AccordoCommercialeListino_AccordoCommerciale]
GO

ALTER TABLE [dbo].[AccordoCommercialeListino]  WITH NOCHECK ADD  CONSTRAINT [FK_AccordoCommercialeListino_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[AccordoCommercialeListino] CHECK CONSTRAINT [FK_AccordoCommercialeListino_Listino]
GO

ALTER TABLE [dbo].[AccordoCommercialeListino]  WITH NOCHECK ADD  CONSTRAINT [FK_AccordoCommercialeListino_Tariffario] FOREIGN KEY([TariffarioID])
REFERENCES [pricing].[Tariffario] ([TariffarioID])
GO

ALTER TABLE [dbo].[AccordoCommercialeListino] CHECK CONSTRAINT [FK_AccordoCommercialeListino_Tariffario]
GO

