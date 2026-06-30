/****** Object:  Table [dbo].[ModelloMezzoGruppo]    Script Date: 30/06/2026 09:43:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ModelloMezzoGruppo](
	[ModelloMezzoGruppoID] [int] IDENTITY(1,1) NOT NULL,
	[GruppoID] [int] NOT NULL,
	[ModelloMezzoID] [int] NOT NULL,
 CONSTRAINT [PK_ModelloMezzoGruppo] PRIMARY KEY CLUSTERED 
(
	[ModelloMezzoGruppoID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ModelloMezzoGruppo]  WITH CHECK ADD  CONSTRAINT [FK_ModelloMezzoGruppo_Gruppo] FOREIGN KEY([GruppoID])
REFERENCES [dbo].[Gruppo] ([GruppoID])
GO

ALTER TABLE [dbo].[ModelloMezzoGruppo] CHECK CONSTRAINT [FK_ModelloMezzoGruppo_Gruppo]
GO

ALTER TABLE [dbo].[ModelloMezzoGruppo]  WITH CHECK ADD  CONSTRAINT [FK_ModelloMezzoGruppo_ModelloMezzo] FOREIGN KEY([ModelloMezzoID])
REFERENCES [dbo].[ModelloMezzo] ([ModelloMezzoID])
GO

ALTER TABLE [dbo].[ModelloMezzoGruppo] CHECK CONSTRAINT [FK_ModelloMezzoGruppo_ModelloMezzo]
GO

