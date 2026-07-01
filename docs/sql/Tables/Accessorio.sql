/****** Object:  Table [dbo].[Accessorio]    Script Date: 30/06/2026 14:23:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Accessorio](
	[AccessorioID] [int] NOT NULL,
	[ChiaveAccessorio] [nvarchar](100) NOT NULL,
	[Descrizione] [text] NULL,
	[Ordinamento] [smallint] NULL,
 CONSTRAINT [PK_Accessorio] PRIMARY KEY CLUSTERED 
(
	[AccessorioID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

