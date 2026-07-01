/****** Object:  Table [dbo].[Listino]    Script Date: 30/06/2026 14:10:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Listino](
	[ListinoID] [int] IDENTITY(1,1) NOT NULL,
	[Tipo] [varchar](10) NOT NULL,
	[Descrizione] [varchar](100) NOT NULL,
	[DataCreazione] [datetime] NOT NULL,
	[InizioValidita] [datetime] NULL,
	[FineValidita] [datetime] NULL,
	[IsGrandiGruppi] [bit] NOT NULL,
	[Ordinamento] [tinyint] NOT NULL,
	[SempreAttivo] [bit] NOT NULL,
	[Stato] [tinyint] NOT NULL,
	[ListinoRaggruppamentoID] [int] NULL,
	[OperatoreID] [int] NULL,
	[DataUltimaModifica] [datetime] NULL,
	[ListinoIDPadre] [int] NULL,
	[Versione] [smallint] NULL,
	[VersioneNome] [nvarchar](100) NULL,
	[DataCreazioneOriginale] [datetime] NULL,
	[StatoVisibilita] [tinyint] NULL,
	[GuidListino] [uniqueidentifier] NOT NULL,
	[DataInserimento] [datetime] NULL,
	[TariffarioID] [int] NOT NULL,
	[EsclusioneWalkIn] [bit] NOT NULL,
	[TolleranzaOraria] [smallint] NULL,
	[ProdottoOrario] [bit] NOT NULL,
	[ProdottoOrarioTipoImporto] [char](1) NULL,
	[ProdottoOrarioPercentuale] [decimal](5, 2) NULL,
	[ProdottoOrarioImporto] [money] NULL,
	[DurataMinimaNolo] [int] NULL,
	[DurataMassimaNolo] [int] NULL,
	[DurataMinimaAddebitabile] [int] NULL,
	[TipologiaCalcoloWeekend] [tinyint] NOT NULL,
	[GiornoInizioWeekend] [tinyint] NULL,
	[GiornoFineWeekend] [tinyint] NULL,
	[OrarioInizioWeekend] [time](7) NULL,
	[OrarioFineWeekend] [time](7) NULL,
	[FullCredit] [bit] NOT NULL,
 CONSTRAINT [PK_Listino] PRIMARY KEY CLUSTERED 
(
	[ListinoID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Listino] ADD  CONSTRAINT [DF_Listino_IsGrandiGruppi]  DEFAULT ((0)) FOR [IsGrandiGruppi]
GO

ALTER TABLE [dbo].[Listino] ADD  CONSTRAINT [DF_Listino_Ordinamento]  DEFAULT ((0)) FOR [Ordinamento]
GO

ALTER TABLE [dbo].[Listino] ADD  CONSTRAINT [DF_Listino_SempreAttivo]  DEFAULT ((0)) FOR [SempreAttivo]
GO

ALTER TABLE [dbo].[Listino] ADD  CONSTRAINT [DF_Listino_Stato]  DEFAULT ((1)) FOR [Stato]
GO

ALTER TABLE [dbo].[Listino] ADD  CONSTRAINT [DF_Listino_Versione]  DEFAULT ((1)) FOR [Versione]
GO

ALTER TABLE [dbo].[Listino] ADD  DEFAULT ((0)) FOR [TipologiaCalcoloWeekend]
GO

ALTER TABLE [dbo].[Listino] ADD  DEFAULT ((0)) FOR [FullCredit]
GO

ALTER TABLE [dbo].[Listino]  WITH CHECK ADD  CONSTRAINT [FK_Listino_Listino] FOREIGN KEY([ListinoIDPadre])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[Listino] CHECK CONSTRAINT [FK_Listino_Listino]
GO

ALTER TABLE [dbo].[Listino]  WITH CHECK ADD  CONSTRAINT [FK_Listino_ListinoRaggruppamento] FOREIGN KEY([ListinoRaggruppamentoID])
REFERENCES [dbo].[ListinoRaggruppamento] ([ListinoRaggruppamentoID])
GO

ALTER TABLE [dbo].[Listino] CHECK CONSTRAINT [FK_Listino_ListinoRaggruppamento]
GO

ALTER TABLE [dbo].[Listino]  WITH CHECK ADD  CONSTRAINT [FK_Listino_Operatore] FOREIGN KEY([OperatoreID])
REFERENCES [dbo].[Operatore] ([OperatoreID])
GO

ALTER TABLE [dbo].[Listino] CHECK CONSTRAINT [FK_Listino_Operatore]
GO

ALTER TABLE [dbo].[Listino]  WITH CHECK ADD  CONSTRAINT [FK_Listino_Tariffario] FOREIGN KEY([TariffarioID])
REFERENCES [pricing].[Tariffario] ([TariffarioID])
GO

ALTER TABLE [dbo].[Listino] CHECK CONSTRAINT [FK_Listino_Tariffario]
GO

