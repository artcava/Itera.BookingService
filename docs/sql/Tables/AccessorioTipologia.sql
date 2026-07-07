/****** Object:  Table [dbo].[AccessorioTipologia]    Script Date: 07/07/2026 09:16:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccessorioTipologia](
	[AccessorioTipologiaID] [smallint] IDENTITY(1,1) NOT NULL,
	[AccessorioCategoriaID] [smallint] NOT NULL,
	[Codice] [varchar](10) NOT NULL,
	[BrandID] [smallint] NULL,
	[Obbligatorio] [bit] NOT NULL,
	[IvaID] [smallint] NULL,
	[OneriAptRw] [bit] NOT NULL,
	[ImportoForzato] [bit] NOT NULL,
	[InseribileFiliale] [bit] NOT NULL,
	[ModificabileFiliale] [bit] NOT NULL,
	[PrepagamentoWeb] [bit] NOT NULL,
	[PrepagamentoContactCenter] [bit] NOT NULL,
	[VendibilitaWeb] [bit] NOT NULL,
	[VendibilitaContactCenter] [bit] NOT NULL,
	[VendibilitaFiliale] [bit] NOT NULL,
	[TipologiaVoceFatturaID] [tinyint] NOT NULL,
	[PercentualeShort] [decimal](5, 2) NOT NULL,
	[PercentualeOpen] [decimal](5, 2) NOT NULL,
	[PercentualeAccessorio] [decimal](5, 2) NOT NULL,
	[StatoID] [tinyint] NOT NULL,
	[DataInizioValidita] [datetime] NULL,
	[DataFineValidita] [datetime] NULL,
	[SplitInFattura] [bit] NOT NULL,
	[PercentualeSplit] [decimal](5, 2) NOT NULL,
	[ImportoPenale] [money] NULL,
	[TipologiaVoceFatturaIDPenale] [tinyint] NULL,
	[Preselezionato] [bit] NOT NULL,
	[MomentoVendibilita] [char](2) NULL,
	[AccessorioTipologiaIDPenale] [smallint] NULL,
	[Vendibile] [bit] NOT NULL,
 CONSTRAINT [PK_AccessorioTipologia] PRIMARY KEY CLUSTERED 
(
	[AccessorioTipologiaID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AccessorioTipologia] ADD  DEFAULT ((0)) FOR [Vendibile]
GO

ALTER TABLE [dbo].[AccessorioTipologia]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioTipologia_AccessorioCategoria] FOREIGN KEY([AccessorioCategoriaID])
REFERENCES [dbo].[AccessorioCategoria] ([AccessorioCategoriaID])
GO

ALTER TABLE [dbo].[AccessorioTipologia] CHECK CONSTRAINT [FK_AccessorioTipologia_AccessorioCategoria]
GO

ALTER TABLE [dbo].[AccessorioTipologia]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioTipologia_AccessorioTipologia] FOREIGN KEY([AccessorioTipologiaIDPenale])
REFERENCES [dbo].[AccessorioTipologia] ([AccessorioTipologiaID])
GO

ALTER TABLE [dbo].[AccessorioTipologia] CHECK CONSTRAINT [FK_AccessorioTipologia_AccessorioTipologia]
GO

ALTER TABLE [dbo].[AccessorioTipologia]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioTipologia_Brand] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brand] ([BrandID])
GO

ALTER TABLE [dbo].[AccessorioTipologia] CHECK CONSTRAINT [FK_AccessorioTipologia_Brand]
GO

ALTER TABLE [dbo].[AccessorioTipologia]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioTipologia_Iva] FOREIGN KEY([IvaID])
REFERENCES [dbo].[Iva] ([IvaID])
GO

ALTER TABLE [dbo].[AccessorioTipologia] CHECK CONSTRAINT [FK_AccessorioTipologia_Iva]
GO

ALTER TABLE [dbo].[AccessorioTipologia]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioTipologia_TipologiaVoceFattura] FOREIGN KEY([TipologiaVoceFatturaID])
REFERENCES [dbo].[TipologiaVoceFattura] ([TipologiaVoceFatturaID])
GO

ALTER TABLE [dbo].[AccessorioTipologia] CHECK CONSTRAINT [FK_AccessorioTipologia_TipologiaVoceFattura]
GO

ALTER TABLE [dbo].[AccessorioTipologia]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioTipologia_TipologiaVoceFattura1] FOREIGN KEY([TipologiaVoceFatturaIDPenale])
REFERENCES [dbo].[TipologiaVoceFattura] ([TipologiaVoceFatturaID])
GO

ALTER TABLE [dbo].[AccessorioTipologia] CHECK CONSTRAINT [FK_AccessorioTipologia_TipologiaVoceFattura1]
GO

