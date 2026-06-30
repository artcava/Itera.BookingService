/****** Object:  Table [dbo].[Mezzo]    Script Date: 29/06/2026 17:11:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Mezzo](
	[CodiceMezzo] [varchar](20) NOT NULL,
	[ModelloMezzoID] [int] NOT NULL,
	[TipoMezzoID] [smallint] NOT NULL,
	[StatoMezzoID] [smallint] NOT NULL,
	[Targa] [varchar](100) NULL,
	[Telaio] [varchar](100) NULL,
	[CodiceAutoradio] [varchar](20) NULL,
	[KeyCode] [varchar](20) NULL,
	[FAP] [bit] NOT NULL,
	[KmEffettuati] [int] NULL,
	[ColoreInterno] [varchar](50) NULL,
	[ColoreEsterno] [varchar](50) NULL,
	[ContrattoMezzoID] [int] NOT NULL,
	[AssicurazioneMezzoID] [int] NOT NULL,
	[AllestimentoMezzoID] [smallint] NULL,
	[Pianalatura] [bit] NOT NULL,
	[AllestimentoOK] [bit] NOT NULL,
	[LoghiEsterni] [bit] NOT NULL,
	[FilialeID] [int] NULL,
	[StatoCondizioneMezzoID] [tinyint] NULL,
	[ProgressivoAcquisto] [int] NOT NULL,
	[CodiceMezzoFinale] [varchar](100) NULL,
	[DataEvasione] [datetime] NULL,
	[CarburanteMezzoID] [tinyint] NULL,
	[DataFineContratto] [datetime] NULL,
	[IsEsportato] [bit] NULL,
	[CodiceMezzoVisualizzato] [varchar](100) NULL,
	[Note] [text] NULL,
	[SubCodice] [nvarchar](100) NULL,
	[ProprietaFisica] [int] NOT NULL,
	[ProprietaLogica] [int] NOT NULL,
	[DataImmatricolazione] [datetime] NULL,
	[Hold] [bit] NULL,
	[DataFineCanoni] [datetime] NULL,
	[DataModificaHold] [datetime] NULL,
	[FranchiseID] [int] NULL,
	[BrandID] [smallint] NOT NULL,
	[Evadibile] [bit] NULL,
	[Lucchetto] [bit] NOT NULL,
	[ValidoPerGdpr] [bit] NOT NULL,
	[SottoFacilityID] [int] NULL,
	[GommeTermiche] [bit] NULL,
 CONSTRAINT [PK_Mezzo] PRIMARY KEY CLUSTERED 
(
	[CodiceMezzo] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Mezzo] ADD  CONSTRAINT [DF_Mezzo_FAP]  DEFAULT ((0)) FOR [FAP]
GO

ALTER TABLE [dbo].[Mezzo] ADD  CONSTRAINT [DF_Mezzo_KmEffettuati]  DEFAULT ((0)) FOR [KmEffettuati]
GO

ALTER TABLE [dbo].[Mezzo] ADD  CONSTRAINT [DF_Mezzo_Pianalatura]  DEFAULT ((0)) FOR [Pianalatura]
GO

ALTER TABLE [dbo].[Mezzo] ADD  CONSTRAINT [DF_Mezzo_AllestimentoOK]  DEFAULT ((0)) FOR [AllestimentoOK]
GO

ALTER TABLE [dbo].[Mezzo] ADD  CONSTRAINT [DF_Mezzo_LoghiEsterni]  DEFAULT ((0)) FOR [LoghiEsterni]
GO

ALTER TABLE [dbo].[Mezzo] ADD  DEFAULT ((0)) FOR [Hold]
GO

ALTER TABLE [dbo].[Mezzo] ADD  DEFAULT ((0)) FOR [Evadibile]
GO

ALTER TABLE [dbo].[Mezzo] ADD  CONSTRAINT [Lucchetto_Default]  DEFAULT ((0)) FOR [Lucchetto]
GO

ALTER TABLE [dbo].[Mezzo] ADD  DEFAULT ((1)) FOR [ValidoPerGdpr]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_AllestimentoMezzo] FOREIGN KEY([AllestimentoMezzoID])
REFERENCES [dbo].[AllestimentoMezzo] ([AllestimentoMezzoID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_AllestimentoMezzo]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_AssicurazioneMezzo] FOREIGN KEY([AssicurazioneMezzoID])
REFERENCES [dbo].[AssicurazioneMezzo] ([AssicurazioneMezzoID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_AssicurazioneMezzo]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_Brand] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brand] ([BrandID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_Brand]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_CarburanteMezzo] FOREIGN KEY([CarburanteMezzoID])
REFERENCES [dbo].[CarburanteMezzo] ([CarburanteMezzoID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_CarburanteMezzo]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_ContrattoMezzo] FOREIGN KEY([ContrattoMezzoID])
REFERENCES [dbo].[ContrattoMezzo] ([ContrattoMezzoID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_ContrattoMezzo]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_Filiale] FOREIGN KEY([FilialeID])
REFERENCES [dbo].[Filiale] ([FilialeID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_Filiale]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_ModelloMezzo] FOREIGN KEY([ModelloMezzoID])
REFERENCES [dbo].[ModelloMezzo] ([ModelloMezzoID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_ModelloMezzo]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_ProprietaFisica] FOREIGN KEY([ProprietaFisica])
REFERENCES [dbo].[Franchise] ([FranchiseID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_ProprietaFisica]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_ProprietaLogica] FOREIGN KEY([ProprietaLogica])
REFERENCES [dbo].[Franchise] ([FranchiseID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_ProprietaLogica]
GO

ALTER TABLE [dbo].[Mezzo]  WITH NOCHECK ADD  CONSTRAINT [FK_Mezzo_SottoFacility] FOREIGN KEY([SottoFacilityID])
REFERENCES [dbo].[SottoFacility] ([SottoFacilityID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_SottoFacility]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_StatoCondizioneMezzo] FOREIGN KEY([StatoCondizioneMezzoID])
REFERENCES [dbo].[StatoCondizioneMezzo] ([StatoCondizioneMezzoID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_StatoCondizioneMezzo]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_StatoMezzo] FOREIGN KEY([StatoMezzoID])
REFERENCES [dbo].[StatoMezzo] ([StatoMezzoID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_StatoMezzo]
GO

ALTER TABLE [dbo].[Mezzo]  WITH CHECK ADD  CONSTRAINT [FK_Mezzo_TipoMezzo] FOREIGN KEY([TipoMezzoID])
REFERENCES [dbo].[TipoMezzo] ([TipoMezzoID])
GO

ALTER TABLE [dbo].[Mezzo] CHECK CONSTRAINT [FK_Mezzo_TipoMezzo]
GO

