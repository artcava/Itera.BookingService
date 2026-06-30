/****** Object:  Table [dbo].[ListinoBrand]    Script Date: 30/06/2026 15:05:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ListinoBrand](
	[ListinoBrandID] [int] IDENTITY(1,1) NOT NULL,
	[BrandID] [smallint] NOT NULL,
	[ListinoID] [int] NOT NULL,
	[DataInserimento] [datetime] NULL,
	[DataUltimaModifica] [datetime] NULL,
 CONSTRAINT [PK_ListinoBrandID] PRIMARY KEY CLUSTERED 
(
	[ListinoBrandID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ListinoBrand]  WITH CHECK ADD  CONSTRAINT [FK_ListinoBrand_Brand] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brand] ([BrandID])
GO

ALTER TABLE [dbo].[ListinoBrand] CHECK CONSTRAINT [FK_ListinoBrand_Brand]
GO

ALTER TABLE [dbo].[ListinoBrand]  WITH NOCHECK ADD  CONSTRAINT [FK_ListinoBrand_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[ListinoBrand] CHECK CONSTRAINT [FK_ListinoBrand_Listino]
GO

