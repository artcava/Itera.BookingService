/****** Object:  Table [dbo].[ListinoFranchigiaTipologia]    Script Date: 14/07/2026 16:35:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ListinoFranchigiaTipologia](
	[ListinoID] [int] NOT NULL,
	[TipologiaFranchigiaID] [nvarchar](3) NOT NULL,
	[StatoID] [smallint] NOT NULL,
	[StatoInclusione] [varchar](3) NULL,
 CONSTRAINT [PK_ListinoFranchigiaTipologia_1] PRIMARY KEY CLUSTERED 
(
	[ListinoID] ASC,
	[TipologiaFranchigiaID] ASC
)WITH (STATISTICS_NORECOMPUTE = ON, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ListinoFranchigiaTipologia]  WITH NOCHECK ADD  CONSTRAINT [FK_ListinoFranchigiaTipologia_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[ListinoFranchigiaTipologia] CHECK CONSTRAINT [FK_ListinoFranchigiaTipologia_Listino]
GO

ALTER TABLE [dbo].[ListinoFranchigiaTipologia]  WITH NOCHECK ADD  CONSTRAINT [FK_ListinoFranchigiaTipologia_TipologiaFranchigia] FOREIGN KEY([TipologiaFranchigiaID])
REFERENCES [dbo].[TipologiaFranchigia] ([TipologiaFranchigiaID])
GO

ALTER TABLE [dbo].[ListinoFranchigiaTipologia] CHECK CONSTRAINT [FK_ListinoFranchigiaTipologia_TipologiaFranchigia]
GO

ALTER TABLE [dbo].[ListinoFranchigiaTipologia]  WITH NOCHECK ADD  CONSTRAINT [CK_ListinoFranchigiaTipologia_StatoInclusione] CHECK  (([StatoInclusione]='I' OR [StatoInclusione]='E'))
GO

ALTER TABLE [dbo].[ListinoFranchigiaTipologia] CHECK CONSTRAINT [CK_ListinoFranchigiaTipologia_StatoInclusione]
GO

