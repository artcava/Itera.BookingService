/****** Object:  Table [dbo].[WsUser]    Script Date: 29/06/2026 15:40:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WsUser](
	[WsUserID] [int] NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[SecretWord] [varchar](50) NOT NULL,
	[Descrizione] [varchar](250) NULL,
	[AccountTest] [bit] NULL,
	[AccettaSegmentoNonVendibile] [bit] NULL,
	[DisponibilitaMacroArea] [bit] NULL,
	[ListinoScontisticaID] [int] NULL,
	[BrandID] [smallint] NULL,
	[DisponibilitaRaggruppamento] [bit] NULL,
 CONSTRAINT [PK_WsUser] PRIMARY KEY CLUSTERED 
(
	[WsUserID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WsUser]  WITH NOCHECK ADD  CONSTRAINT [FK_WsUser_Brand] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brand] ([BrandID])
GO

ALTER TABLE [dbo].[WsUser] CHECK CONSTRAINT [FK_WsUser_Brand]
GO

ALTER TABLE [dbo].[WsUser]  WITH CHECK ADD  CONSTRAINT [FK_WsUser_ListinoScontistica] FOREIGN KEY([ListinoScontisticaID])
REFERENCES [dbo].[ListinoScontistica] ([ListinoScontisticaID])
GO

ALTER TABLE [dbo].[WsUser] CHECK CONSTRAINT [FK_WsUser_ListinoScontistica]
GO

