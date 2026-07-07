/****** Object:  Table [pricing].[TariffaRdv]    Script Date: 07/07/2026 11:31:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [pricing].[TariffaRdv](
	[TariffaRdvID] [int] IDENTITY(1,1) NOT NULL,
	[TariffarioID] [int] NOT NULL,
	[AccessorioTipologiaID] [smallint] NOT NULL,
	[DataStart] [datetime] NOT NULL,
	[DataEnd] [datetime] NOT NULL,
	[BreakEven] [smallint] NOT NULL,
	[MinGiorniApplicabilita] [smallint] NOT NULL,
	[MaxGiorniApplicabilita] [smallint] NOT NULL,
	[Percentuale] [decimal](5, 2) NULL,
	[ImportoFisso] [money] NULL,
	[TipoImporto] [varchar](3) NOT NULL,
	[ImportoGiornoExtra] [money] NULL,
	[ImportoMinAddebitabile] [money] NULL,
	[ImportoMaxAddebitabile] [money] NULL,
	[MaxGiorniAddebitabili] [smallint] NULL,
	[Tolleranza] [decimal](5, 2) NULL,
	[StatoInclusione] [varchar](3) NULL,
	[Incasso] [varchar](3) NULL,
	[StatoID] [tinyint] NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[DataUltimaModifica] [datetime] NULL,
 CONSTRAINT [PK_TariffaRdv] PRIMARY KEY CLUSTERED 
(
	[TariffaRdvID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [FK_TariffaRdv_AccessorioTipologia] FOREIGN KEY([AccessorioTipologiaID])
REFERENCES [dbo].[AccessorioTipologia] ([AccessorioTipologiaID])
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [FK_TariffaRdv_AccessorioTipologia]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [FK_TariffaRdv_Tariffario] FOREIGN KEY([TariffarioID])
REFERENCES [pricing].[Tariffario] ([TariffarioID])
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [FK_TariffaRdv_Tariffario]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [CK_TariffaRdv_BreakEvenMaggioreDi0] CHECK  (([BreakEven]>=(0)))
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [CK_TariffaRdv_BreakEvenMaggioreDi0]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [CK_TariffaRdv_DataEndMaggioreDiDataStart] CHECK  (([DataEnd]>[DataStart]))
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [CK_TariffaRdv_DataEndMaggioreDiDataStart]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [CK_TariffaRdv_ImportoFissoPercentuale] CHECK  (([ImportoFisso] IS NULL AND [Percentuale] IS NOT NULL OR [ImportoFisso] IS NOT NULL AND [Percentuale] IS NULL))
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [CK_TariffaRdv_ImportoFissoPercentuale]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [CK_TariffaRdv_Inclusione] CHECK  (([Incasso]='A' OR [Incasso]='D'))
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [CK_TariffaRdv_Inclusione]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [CK_TariffaRdv_MaxGiorniApplicabilitaMaggioreDiMinGiorniApplicabilita] CHECK  (([MaxGiorniApplicabilita]>=[MinGiorniApplicabilita]))
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [CK_TariffaRdv_MaxGiorniApplicabilitaMaggioreDiMinGiorniApplicabilita]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [CK_TariffaRdv_MinGiorniApplicabilitaMaggioreDi0] CHECK  (([MinGiorniApplicabilita]>(0)))
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [CK_TariffaRdv_MinGiorniApplicabilitaMaggioreDi0]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [CK_TariffaRdv_StatoInclusione] CHECK  (([StatoInclusione]='I' OR [StatoInclusione]='E'))
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [CK_TariffaRdv_StatoInclusione]
GO

ALTER TABLE [pricing].[TariffaRdv]  WITH NOCHECK ADD  CONSTRAINT [CK_TariffaRdv_TipoImporto] CHECK  (([TipoImporto]='U' OR [TipoImporto]='F' OR [TipoImporto]='X'))
GO

ALTER TABLE [pricing].[TariffaRdv] CHECK CONSTRAINT [CK_TariffaRdv_TipoImporto]
GO

