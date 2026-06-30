/****** Object:  Table [dbo].[ModelloMezzo]    Script Date: 30/06/2026 09:41:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ModelloMezzo](
	[ModelloMezzoID] [int] IDENTITY(1,1) NOT NULL,
	[CodiceSegmento] [varchar](3) NULL,
	[CodiceCategoria] [varchar](5) NULL,
	[MarcaID] [smallint] NOT NULL,
	[AlimentazioneModelloID] [smallint] NOT NULL,
	[Serbatoio] [smallint] NULL,
	[Descrizione] [varchar](50) NOT NULL,
	[NomeImmagine] [varchar](100) NULL,
	[Passo] [float] NULL,
	[LunghezzaEsterna] [float] NULL,
	[AltezzaEsterna] [float] NULL,
	[LarghezzaEsterna] [float] NULL,
	[Peso] [smallint] NULL,
	[LarghezzaPassaruote] [float] NULL,
	[LunghezzaInterna] [float] NULL,
	[AltezzaInterna] [float] NULL,
	[LarghezzaInterna] [float] NULL,
	[Portata] [smallint] NULL,
	[VolumeCarico] [smallint] NULL,
	[NumeroPallets] [smallint] NULL,
	[AltezzaCarico] [float] NULL,
	[MisurePortaPosteriore] [float] NULL,
	[MisurePortaLaterale] [float] NULL,
	[Cilindrata] [smallint] NULL,
	[Euro] [char](2) NULL,
	[Km] [int] NULL,
	[CV] [smallint] NULL,
	[CapacitaSerbatoio] [smallint] NULL,
	[NumeroPorte] [tinyint] NULL,
	[NumeroPosti] [tinyint] NULL,
	[Autoradio] [bit] NULL,
	[AriaCondizionata] [bit] NULL,
	[Airbag] [bit] NULL,
	[Abs] [bit] NULL,
	[Eps] [bit] NULL,
	[MisuraGomme] [varchar](50) NULL,
	[RuotaScorta] [bit] NULL,
	[KitGonfiaggio] [bit] NULL,
	[DataModifica] [datetime] NOT NULL,
	[KmTagliandoFreni] [int] NULL,
	[SogliaKmTagliandoFreni] [int] NULL,
	[KmTagliandoOlio] [int] NULL,
	[SogliaKmTagliandoOlio] [int] NULL,
	[Ruotino] [bit] NULL,
	[NomeFileSchedaTecnica] [varchar](150) NULL,
	[VisibilitaSito] [bit] NULL,
	[ACRISSCode] [varchar](100) NULL,
	[NumeroPostiCarrozzina] [tinyint] NULL,
	[NumeroPostiMobility] [tinyint] NULL,
	[PedanaSollevatriceDoppioBraccio] [bit] NOT NULL,
	[DescrizioneMobilitySitoWeb_ITA] [nvarchar](max) NULL,
	[DescrizioneMobilitySitoWeb_ENG] [nvarchar](max) NULL,
 CONSTRAINT [PK_ModelloMezzo] PRIMARY KEY CLUSTERED 
(
	[ModelloMezzoID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ModelloMezzo] ADD  CONSTRAINT [DF_ModelloMezzo_DataModifica]  DEFAULT (getdate()) FOR [DataModifica]
GO

ALTER TABLE [dbo].[ModelloMezzo] ADD  DEFAULT ((0)) FOR [PedanaSollevatriceDoppioBraccio]
GO

ALTER TABLE [dbo].[ModelloMezzo]  WITH NOCHECK ADD  CONSTRAINT [FK_ModelloMezzo_AlimentazioneModello] FOREIGN KEY([AlimentazioneModelloID])
REFERENCES [dbo].[AlimentazioneModello] ([AlimentazioneModelloID])
GO

ALTER TABLE [dbo].[ModelloMezzo] CHECK CONSTRAINT [FK_ModelloMezzo_AlimentazioneModello]
GO

ALTER TABLE [dbo].[ModelloMezzo]  WITH CHECK ADD  CONSTRAINT [FK_ModelloMezzo_CategoriaSegmento] FOREIGN KEY([CodiceCategoria])
REFERENCES [dbo].[CategoriaSegmento] ([CodiceCategoria])
GO

ALTER TABLE [dbo].[ModelloMezzo] CHECK CONSTRAINT [FK_ModelloMezzo_CategoriaSegmento]
GO

ALTER TABLE [dbo].[ModelloMezzo]  WITH CHECK ADD  CONSTRAINT [FK_ModelloMezzo_EuroModello] FOREIGN KEY([Euro])
REFERENCES [dbo].[EuroModello] ([CodiceEuro])
GO

ALTER TABLE [dbo].[ModelloMezzo] CHECK CONSTRAINT [FK_ModelloMezzo_EuroModello]
GO

ALTER TABLE [dbo].[ModelloMezzo]  WITH CHECK ADD  CONSTRAINT [FK_ModelloMezzo_Marca] FOREIGN KEY([MarcaID])
REFERENCES [dbo].[Marca] ([MarcaID])
GO

ALTER TABLE [dbo].[ModelloMezzo] CHECK CONSTRAINT [FK_ModelloMezzo_Marca]
GO

ALTER TABLE [dbo].[ModelloMezzo]  WITH CHECK ADD  CONSTRAINT [FK_ModelloMezzo_SegmentoModello1] FOREIGN KEY([CodiceSegmento])
REFERENCES [dbo].[SegmentoModello] ([CodiceSegmento])
GO

ALTER TABLE [dbo].[ModelloMezzo] CHECK CONSTRAINT [FK_ModelloMezzo_SegmentoModello1]
GO

