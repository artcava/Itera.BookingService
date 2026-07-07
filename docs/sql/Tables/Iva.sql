/****** Object:  Table [dbo].[Iva]    Script Date: 06/07/2026 11:42:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Iva](
	[IvaID] [smallint] IDENTITY(1,1) NOT NULL,
	[Percentuale] [decimal](5, 2) NOT NULL,
	[ValidaDal] [datetime] NOT NULL,
	[ValidaAl] [datetime] NULL,
	[Descrizione] [varchar](500) NULL,
	[Ordinamento] [int] NULL,
	[Sistema] [bit] NOT NULL,
	[FatturaOmaggio] [bit] NULL,
	[DescrizioneSuFattura] [varchar](500) NULL,
	[NotaAggiuntivaSuFatturaITA] [nvarchar](500) NULL,
	[NotaAggiuntivaSuFatturaENG] [nvarchar](500) NULL,
	[IvaIDSostituzione365Plus] [smallint] NULL,
	[SplitPayment] [bit] NOT NULL,
 CONSTRAINT [PK_Iva] PRIMARY KEY CLUSTERED 
(
	[IvaID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Iva] ADD  CONSTRAINT [DF_Iva_Sistema]  DEFAULT ((1)) FOR [Sistema]
GO

ALTER TABLE [dbo].[Iva] ADD  DEFAULT ((0)) FOR [SplitPayment]
GO

ALTER TABLE [dbo].[Iva]  WITH NOCHECK ADD  CONSTRAINT [FK_Iva_Iva365Plus] FOREIGN KEY([IvaIDSostituzione365Plus])
REFERENCES [dbo].[Iva] ([IvaID])
GO

ALTER TABLE [dbo].[Iva] CHECK CONSTRAINT [FK_Iva_Iva365Plus]
GO

