/****** Object:  Table [dbo].[CategoriaFattura]    Script Date: 07/07/2026 09:41:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CategoriaFattura](
	[CategoriaFatturaID] [tinyint] NOT NULL,
	[Descrizione] [nvarchar](50) NOT NULL,
	[IvaIDPredefinita] [smallint] NULL,
 CONSTRAINT [PK_CategoriaFattura] PRIMARY KEY CLUSTERED 
(
	[CategoriaFatturaID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CategoriaFattura]  WITH CHECK ADD  CONSTRAINT [FK_CategoriaFattura_Iva] FOREIGN KEY([IvaIDPredefinita])
REFERENCES [dbo].[Iva] ([IvaID])
GO

ALTER TABLE [dbo].[CategoriaFattura] CHECK CONSTRAINT [FK_CategoriaFattura_Iva]
GO

