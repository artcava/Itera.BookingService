/****** Object:  Table [dbo].[ListinoKm]    Script Date: 30/06/2026 14:14:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ListinoKm](
	[ListinoKmID] [int] IDENTITY(1,1) NOT NULL,
	[ListinoGiorniID] [int] NOT NULL,
	[Km] [int] NOT NULL,
	[Ordinamento] [smallint] NOT NULL,
	[IsVisible] [bit] NULL,
 CONSTRAINT [PK_ListinoKm] PRIMARY KEY CLUSTERED 
(
	[ListinoKmID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ListinoKm]  WITH CHECK ADD  CONSTRAINT [FK_ListinoKm_ListinoGiorni] FOREIGN KEY([ListinoGiorniID])
REFERENCES [dbo].[ListinoGiorni] ([ListinoGiorniID])
GO

ALTER TABLE [dbo].[ListinoKm] CHECK CONSTRAINT [FK_ListinoKm_ListinoGiorni]
GO

