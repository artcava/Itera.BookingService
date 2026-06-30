/****** Object:  Table [dbo].[SegmentoModelloMezzoSpeciale]    Script Date: 30/06/2026 09:43:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SegmentoModelloMezzoSpeciale](
	[SegmentoModelloMezzoSpecialeID] [int] IDENTITY(1,1) NOT NULL,
	[CodiceSegmento] [varchar](3) NOT NULL,
 CONSTRAINT [PK_SegmentoModelloMezzoSpeciale] PRIMARY KEY CLUSTERED 
(
	[SegmentoModelloMezzoSpecialeID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SegmentoModelloMezzoSpeciale]  WITH CHECK ADD  CONSTRAINT [FK_SegmentoModelloMezzoSpeciale_SegmentoModello] FOREIGN KEY([CodiceSegmento])
REFERENCES [dbo].[SegmentoModello] ([CodiceSegmento])
GO

ALTER TABLE [dbo].[SegmentoModelloMezzoSpeciale] CHECK CONSTRAINT [FK_SegmentoModelloMezzoSpeciale_SegmentoModello]
GO

