/****** Object:  Table [dbo].[ListinoGiorni]    Script Date: 30/06/2026 14:13:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ListinoGiorni](
	[ListinoGiorniID] [int] IDENTITY(1,1) NOT NULL,
	[ListinoID] [int] NOT NULL,
	[Codice] [varchar](30) NOT NULL,
	[CodiceCategoria] [varchar](5) NOT NULL,
	[Descrizione] [varchar](50) NOT NULL,
	[FasciaStart] [int] NOT NULL,
	[FasciaEnd] [int] NOT NULL,
	[Ordinamento] [smallint] NOT NULL,
	[IsVisible] [bit] NULL,
 CONSTRAINT [PK_ListinoGiorni] PRIMARY KEY CLUSTERED 
(
	[ListinoGiorniID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ListinoGiorni]  WITH CHECK ADD  CONSTRAINT [FK_ListinoGiorni_CategoriaSegmento] FOREIGN KEY([CodiceCategoria])
REFERENCES [dbo].[CategoriaSegmento] ([CodiceCategoria])
GO

ALTER TABLE [dbo].[ListinoGiorni] CHECK CONSTRAINT [FK_ListinoGiorni_CategoriaSegmento]
GO

ALTER TABLE [dbo].[ListinoGiorni]  WITH CHECK ADD  CONSTRAINT [FK_ListinoGiorni_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[ListinoGiorni] CHECK CONSTRAINT [FK_ListinoGiorni_Listino]
GO

