/****** Object:  Table [dbo].[TipologiaFranchigiaCategoria]    Script Date: 14/07/2026 16:36:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TipologiaFranchigiaCategoria](
	[TipologiaFranchigiaCategoriaID] [smallint] NOT NULL,
	[Descrizione] [nvarchar](250) NOT NULL,
	[Codice] [varchar](10) NOT NULL,
	[StatoID] [tinyint] NOT NULL,
 CONSTRAINT [PK_TipologiaFranchigiaCategoria] PRIMARY KEY CLUSTERED 
(
	[TipologiaFranchigiaCategoriaID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

