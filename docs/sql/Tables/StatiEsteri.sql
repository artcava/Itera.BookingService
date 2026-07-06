/****** Object:  Table [dbo].[StatiEsteri]    Script Date: 06/07/2026 09:49:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StatiEsteri](
	[CodiceNazione] [char](3) NOT NULL,
	[ContinenteID] [smallint] NOT NULL,
	[UnioneEuropea] [bit] NOT NULL,
	[DescrizioneItaliana] [varchar](100) NOT NULL,
	[DescrizioneInternazionale] [varchar](100) NOT NULL,
	[CodiceIso] [char](3) NULL,
	[CodiceIso2] [char](2) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StatiEsteri] ADD  CONSTRAINT [DF_StatiEsteri_UnioneEuropea]  DEFAULT ((0)) FOR [UnioneEuropea]
GO

ALTER TABLE [dbo].[StatiEsteri]  WITH CHECK ADD  CONSTRAINT [FK_StatiEsteri_Continente] FOREIGN KEY([ContinenteID])
REFERENCES [dbo].[Continente] ([ContinenteID])
GO

ALTER TABLE [dbo].[StatiEsteri] CHECK CONSTRAINT [FK_StatiEsteri_Continente]
GO

