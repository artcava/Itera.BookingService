/****** Object:  Table [dbo].[AccessorioSegmento]    Script Date: 06/07/2026 11:56:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccessorioSegmento](
	[AccessorioSegmentoID] [int] IDENTITY(1,1) NOT NULL,
	[AccessorioTipologiaID] [smallint] NOT NULL,
	[CodiceSegmento] [varchar](3) NOT NULL,
 CONSTRAINT [PK_AccessorioSegmento] PRIMARY KEY CLUSTERED 
(
	[AccessorioSegmentoID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AccessorioSegmento]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioSegmento_AccessorioTipologia] FOREIGN KEY([AccessorioTipologiaID])
REFERENCES [dbo].[AccessorioTipologia] ([AccessorioTipologiaID])
GO

ALTER TABLE [dbo].[AccessorioSegmento] CHECK CONSTRAINT [FK_AccessorioSegmento_AccessorioTipologia]
GO

ALTER TABLE [dbo].[AccessorioSegmento]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioSegmento_CodiceSegmento] FOREIGN KEY([CodiceSegmento])
REFERENCES [dbo].[SegmentoModello] ([CodiceSegmento])
GO

ALTER TABLE [dbo].[AccessorioSegmento] CHECK CONSTRAINT [FK_AccessorioSegmento_CodiceSegmento]
GO

