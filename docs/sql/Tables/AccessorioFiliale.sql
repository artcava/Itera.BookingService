/****** Object:  Table [dbo].[AccessorioFiliale]    Script Date: 06/07/2026 11:56:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccessorioFiliale](
	[AccessorioFilialeID] [int] IDENTITY(1,1) NOT NULL,
	[AccessorioTipologiaID] [smallint] NOT NULL,
	[FilialeID] [int] NOT NULL,
	[Note] [varchar](50) NULL,
 CONSTRAINT [PK_AccessorioFiliale] PRIMARY KEY CLUSTERED 
(
	[AccessorioFilialeID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AccessorioFiliale]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioFiliale_AccessorioTipologia] FOREIGN KEY([AccessorioTipologiaID])
REFERENCES [dbo].[AccessorioTipologia] ([AccessorioTipologiaID])
GO

ALTER TABLE [dbo].[AccessorioFiliale] CHECK CONSTRAINT [FK_AccessorioFiliale_AccessorioTipologia]
GO

ALTER TABLE [dbo].[AccessorioFiliale]  WITH NOCHECK ADD  CONSTRAINT [FK_AccessorioFiliale_Filiale] FOREIGN KEY([FilialeID])
REFERENCES [dbo].[Filiale] ([FilialeID])
GO

ALTER TABLE [dbo].[AccessorioFiliale] CHECK CONSTRAINT [FK_AccessorioFiliale_Filiale]
GO

