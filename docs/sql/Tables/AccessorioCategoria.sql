/****** Object:  Table [dbo].[AccessorioCategoria]    Script Date: 07/07/2026 09:26:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccessorioCategoria](
	[AccessorioCategoriaID] [smallint] NOT NULL,
	[Descrizione] [nvarchar](250) NOT NULL,
	[Codice] [varchar](10) NOT NULL,
	[BrandID] [smallint] NULL,
	[StatoID] [tinyint] NOT NULL,
 CONSTRAINT [PK_AccessorioCategoria] PRIMARY KEY CLUSTERED 
(
	[AccessorioCategoriaID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AccessorioCategoria]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioCategoria_Brand] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brand] ([BrandID])
GO

ALTER TABLE [dbo].[AccessorioCategoria] CHECK CONSTRAINT [FK_AccessorioCategoria_Brand]
GO

