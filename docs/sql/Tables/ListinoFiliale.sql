/****** Object:  Table [dbo].[ListinoFiliale]    Script Date: 01/07/2026 09:12:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ListinoFiliale](
	[FilialeID] [int] NOT NULL,
	[ListinoID] [int] NOT NULL,
 CONSTRAINT [PK_FilialeListino] PRIMARY KEY CLUSTERED 
(
	[FilialeID] ASC,
	[ListinoID] ASC
)WITH (STATISTICS_NORECOMPUTE = ON, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ListinoFiliale]  WITH CHECK ADD  CONSTRAINT [FK_FilialeListino_Filiale] FOREIGN KEY([FilialeID])
REFERENCES [dbo].[Filiale] ([FilialeID])
GO

ALTER TABLE [dbo].[ListinoFiliale] CHECK CONSTRAINT [FK_FilialeListino_Filiale]
GO

ALTER TABLE [dbo].[ListinoFiliale]  WITH CHECK ADD  CONSTRAINT [FK_FilialeListino_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[ListinoFiliale] CHECK CONSTRAINT [FK_FilialeListino_Listino]
GO

