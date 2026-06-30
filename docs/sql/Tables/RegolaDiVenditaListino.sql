/****** Object:  Table [dbo].[RegolaDiVenditaListino]    Script Date: 30/06/2026 14:15:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RegolaDiVenditaListino](
	[RegolaDiVenditaListinoID] [int] IDENTITY(1,1) NOT NULL,
	[RegolaDiVenditaID] [int] NOT NULL,
	[ListinoID] [int] NULL,
	[ListinoRaggruppamentoID] [int] NULL,
 CONSTRAINT [PK_RegolaDiVenditaListino] PRIMARY KEY CLUSTERED 
(
	[RegolaDiVenditaListinoID] ASC
)WITH (STATISTICS_NORECOMPUTE = ON, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RegolaDiVenditaListino]  WITH CHECK ADD  CONSTRAINT [FK_RegolaDiVenditaListino_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[RegolaDiVenditaListino] CHECK CONSTRAINT [FK_RegolaDiVenditaListino_Listino]
GO

ALTER TABLE [dbo].[RegolaDiVenditaListino]  WITH CHECK ADD  CONSTRAINT [FK_RegolaDiVenditaListino_ListinoRaggruppamento] FOREIGN KEY([ListinoRaggruppamentoID])
REFERENCES [dbo].[ListinoRaggruppamento] ([ListinoRaggruppamentoID])
GO

ALTER TABLE [dbo].[RegolaDiVenditaListino] CHECK CONSTRAINT [FK_RegolaDiVenditaListino_ListinoRaggruppamento]
GO

ALTER TABLE [dbo].[RegolaDiVenditaListino]  WITH CHECK ADD  CONSTRAINT [FK_RegolaDiVenditaListino_RegolaDiVendita] FOREIGN KEY([RegolaDiVenditaID])
REFERENCES [dbo].[RegolaDiVendita] ([RegolaDiVenditaID])
GO

ALTER TABLE [dbo].[RegolaDiVenditaListino] CHECK CONSTRAINT [FK_RegolaDiVenditaListino_RegolaDiVendita]
GO

