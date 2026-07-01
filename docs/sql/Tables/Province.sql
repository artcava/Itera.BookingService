/****** Object:  Table [dbo].[Provincia]    Script Date: 30/06/2026 14:19:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Provincia](
	[CodiceProvincia] [char](3) NOT NULL,
	[CodiceRegione] [char](3) NOT NULL,
	[Denominazione] [nvarchar](255) NOT NULL,
	[SiglaAutomobilistica] [char](2) NOT NULL,
 CONSTRAINT [PK_Provincia] PRIMARY KEY CLUSTERED 
(
	[CodiceProvincia] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Provincia]  WITH CHECK ADD  CONSTRAINT [FK_Provincia_Regione] FOREIGN KEY([CodiceRegione])
REFERENCES [dbo].[Regione] ([CodiceRegione])
GO

ALTER TABLE [dbo].[Provincia] CHECK CONSTRAINT [FK_Provincia_Regione]
GO

