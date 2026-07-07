/****** Object:  Table [dbo].[Brand]    Script Date: 07/07/2026 09:36:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Brand](
	[BrandID] [smallint] NOT NULL,
	[CodiceBrand] [char](3) NOT NULL,
	[Descrizione] [nvarchar](250) NULL,
	[DataInserimento] [datetime] NULL,
	[DataUltimaModifica] [datetime] NULL,
	[DescrizioneSAP] [varchar](15) NULL,
	[DescrizioneMit] [nvarchar](250) NULL,
	[LogoImmagine] [nvarchar](100) NULL,
	[EmailFrom] [nvarchar](100) NULL,
	[EmailFromAlias] [nvarchar](100) NULL,
 CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED 
(
	[BrandID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

