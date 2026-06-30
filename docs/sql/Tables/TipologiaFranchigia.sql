/****** Object:  Table [dbo].[TipologiaFranchigia]    Script Date: 30/06/2026 14:14:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TipologiaFranchigia](
	[TipologiaFranchigiaID] [nvarchar](3) NOT NULL,
	[Descrizione] [nvarchar](100) NOT NULL,
	[TipologiaVoceFatturaID] [tinyint] NULL,
	[Priorita] [smallint] NULL,
	[Note] [nvarchar](500) NULL,
	[TipologiaFranchigiaCategoriaID] [smallint] NULL,
 CONSTRAINT [PK_TipologiaFranchigia] PRIMARY KEY CLUSTERED 
(
	[TipologiaFranchigiaID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TipologiaFranchigia]  WITH NOCHECK ADD  CONSTRAINT [FK_TipologiaFranchigia_TipologiaFranchigiaCategoria] FOREIGN KEY([TipologiaFranchigiaCategoriaID])
REFERENCES [dbo].[TipologiaFranchigiaCategoria] ([TipologiaFranchigiaCategoriaID])
GO

ALTER TABLE [dbo].[TipologiaFranchigia] CHECK CONSTRAINT [FK_TipologiaFranchigia_TipologiaFranchigiaCategoria]
GO

ALTER TABLE [dbo].[TipologiaFranchigia]  WITH NOCHECK ADD  CONSTRAINT [FK_TipologiaFranchigia_TipologiaVoceFattura] FOREIGN KEY([TipologiaVoceFatturaID])
REFERENCES [dbo].[TipologiaVoceFattura] ([TipologiaVoceFatturaID])
GO

ALTER TABLE [dbo].[TipologiaFranchigia] CHECK CONSTRAINT [FK_TipologiaFranchigia_TipologiaVoceFattura]
GO

