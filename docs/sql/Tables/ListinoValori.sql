/****** Object:  Table [dbo].[ListinoValori]    Script Date: 30/06/2026 14:13:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ListinoValori](
	[ListinoValoriID] [int] IDENTITY(1,1) NOT NULL,
	[ListinoID] [int] NULL,
	[ListinoGiorniID] [int] NULL,
	[ListinoKmID] [int] NULL,
	[CodiceCategoria] [varchar](5) NULL,
	[CodiceDurata] [varchar](3) NOT NULL,
	[Giorni] [int] NULL,
	[CodiceSegmento] [varchar](3) NULL,
	[ValoreBase] [money] NULL,
	[ValoreExtra] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[ListinoValoriID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ListinoValori]  WITH CHECK ADD FOREIGN KEY([CodiceSegmento])
REFERENCES [dbo].[SegmentoModello] ([CodiceSegmento])
GO

ALTER TABLE [dbo].[ListinoValori]  WITH CHECK ADD FOREIGN KEY([CodiceCategoria])
REFERENCES [dbo].[CategoriaSegmento] ([CodiceCategoria])
GO

ALTER TABLE [dbo].[ListinoValori]  WITH CHECK ADD FOREIGN KEY([ListinoGiorniID])
REFERENCES [dbo].[ListinoGiorni] ([ListinoGiorniID])
GO

ALTER TABLE [dbo].[ListinoValori]  WITH CHECK ADD FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

