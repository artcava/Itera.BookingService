/****** Object:  Table [dbo].[RipartizioneFatturato]    Script Date: 07/07/2026 09:45:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RipartizioneFatturato](
	[RipartizioneFatturatoID] [char](1) NOT NULL,
	[Descrizione] [varchar](50) NULL,
 CONSTRAINT [PK__RipartizioneFatturato] PRIMARY KEY CLUSTERED 
(
	[RipartizioneFatturatoID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

