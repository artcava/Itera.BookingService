/****** Object:  Table [dbo].[AlimentazioneModello]    Script Date: 30/06/2026 09:41:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AlimentazioneModello](
	[AlimentazioneModelloID] [smallint] IDENTITY(1,1) NOT NULL,
	[Descrizione] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AlimentazioneMezzo] PRIMARY KEY CLUSTERED 
(
	[AlimentazioneModelloID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

