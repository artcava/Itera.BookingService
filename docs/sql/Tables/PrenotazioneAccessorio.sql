/****** Object:  Table [dbo].[PrenotazioneAccessorio]    Script Date: 30/06/2026 14:24:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PrenotazioneAccessorio](
	[PrenotazioneAccessorioID] [int] IDENTITY(1,1) NOT NULL,
	[PrenotazioneID] [int] NOT NULL,
	[AccessorioID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrenotazioneAccessorioID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PrenotazioneAccessorio]  WITH CHECK ADD FOREIGN KEY([AccessorioID])
REFERENCES [dbo].[Accessorio] ([AccessorioID])
GO

ALTER TABLE [dbo].[PrenotazioneAccessorio]  WITH CHECK ADD FOREIGN KEY([PrenotazioneID])
REFERENCES [dbo].[Prenotazione] ([PrenotazioneID])
GO

