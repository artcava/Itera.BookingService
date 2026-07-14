/****** Object:  Table [dbo].[StatoElemento]    Script Date: 14/07/2026 16:36:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StatoElemento](
	[Codice] [smallint] NOT NULL,
	[Descrizione] [nvarchar](100) NULL,
	[Tipologia] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_StatoElemento] PRIMARY KEY CLUSTERED 
(
	[Codice] ASC,
	[Tipologia] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

