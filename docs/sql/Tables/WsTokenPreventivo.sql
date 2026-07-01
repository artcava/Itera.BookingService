/****** Object:  Table [dbo].[WsTokenPreventivo]    Script Date: 30/06/2026 14:16:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WsTokenPreventivo](
	[WsTokenPreventivoID] [int] IDENTITY(1,1) NOT NULL,
	[WsUserID] [int] NOT NULL,
	[Token] [uniqueidentifier] NOT NULL,
	[DataCreazione] [datetime] NOT NULL,
	[DataUltimaModifica] [datetime] NOT NULL,
	[DataFromPreventivo] [datetime] NOT NULL,
	[DataToPreventivo] [datetime] NOT NULL,
	[FilialeID] [int] NOT NULL,
	[FilialeIDDestinazione] [int] NOT NULL,
	[AssicurazioneExtra] [bit] NOT NULL,
	[CodiceCategoria] [varchar](5) NOT NULL,
	[CodiceCoupon] [nvarchar](256) NULL,
	[ObjectDynParam] [varchar](max) NOT NULL,
	[ListinoID] [int] NULL,
	[ListinoScontisticaID] [int] NULL,
	[SourceCampagnaMarketing] [varchar](max) NULL,
	[ObjectEstimate] [nvarchar](max) NULL,
	[PrePagamento] [bit] NULL,
	[PrecedenteWsTokenPreventivoID] [int] NULL,
	[Giorni] [int] NULL,
	[CodiceDurata] [char](1) NULL,
	[VoucherCliente] [nvarchar](100) NULL,
	[AccordoCommercialeID] [int] NULL,
	[YoungDriver] [bit] NULL,
	[SegmentAvailability] [nvarchar](max) NULL,
 CONSTRAINT [PK_WsTokenPreventivo] PRIMARY KEY CLUSTERED 
(
	[WsTokenPreventivoID] ASC
)WITH (STATISTICS_NORECOMPUTE = ON, IGNORE_DUP_KEY = OFF, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[WsTokenPreventivo]  WITH NOCHECK ADD  CONSTRAINT [FK_WsTokenPreventivo_AccordoCommerciale] FOREIGN KEY([AccordoCommercialeID])
REFERENCES [dbo].[AccordoCommerciale] ([AccordoCommercialeID])
GO

ALTER TABLE [dbo].[WsTokenPreventivo] CHECK CONSTRAINT [FK_WsTokenPreventivo_AccordoCommerciale]
GO

ALTER TABLE [dbo].[WsTokenPreventivo]  WITH NOCHECK ADD  CONSTRAINT [FK_WsTokenPreventivo_Durata] FOREIGN KEY([CodiceDurata])
REFERENCES [dbo].[Durata] ([DurataID])
GO

ALTER TABLE [dbo].[WsTokenPreventivo] CHECK CONSTRAINT [FK_WsTokenPreventivo_Durata]
GO

ALTER TABLE [dbo].[WsTokenPreventivo]  WITH CHECK ADD  CONSTRAINT [FK_WsTokenPreventivo_Filiale] FOREIGN KEY([FilialeID])
REFERENCES [dbo].[Filiale] ([FilialeID])
GO

ALTER TABLE [dbo].[WsTokenPreventivo] CHECK CONSTRAINT [FK_WsTokenPreventivo_Filiale]
GO

ALTER TABLE [dbo].[WsTokenPreventivo]  WITH CHECK ADD  CONSTRAINT [FK_WsTokenPreventivo_Filiale1] FOREIGN KEY([FilialeIDDestinazione])
REFERENCES [dbo].[Filiale] ([FilialeID])
GO

ALTER TABLE [dbo].[WsTokenPreventivo] CHECK CONSTRAINT [FK_WsTokenPreventivo_Filiale1]
GO

ALTER TABLE [dbo].[WsTokenPreventivo]  WITH NOCHECK ADD  CONSTRAINT [FK_WsTokenPreventivo_Listino] FOREIGN KEY([ListinoID])
REFERENCES [dbo].[Listino] ([ListinoID])
GO

ALTER TABLE [dbo].[WsTokenPreventivo] CHECK CONSTRAINT [FK_WsTokenPreventivo_Listino]
GO

ALTER TABLE [dbo].[WsTokenPreventivo]  WITH NOCHECK ADD  CONSTRAINT [FK_WsTokenPreventivo_WsTokenPreventivo1] FOREIGN KEY([PrecedenteWsTokenPreventivoID])
REFERENCES [dbo].[WsTokenPreventivo] ([WsTokenPreventivoID])
GO

ALTER TABLE [dbo].[WsTokenPreventivo] CHECK CONSTRAINT [FK_WsTokenPreventivo_WsTokenPreventivo1]
GO

ALTER TABLE [dbo].[WsTokenPreventivo]  WITH CHECK ADD  CONSTRAINT [FK_WsTokenPreventivo_WsUser] FOREIGN KEY([WsUserID])
REFERENCES [dbo].[WsUser] ([WsUserID])
GO

ALTER TABLE [dbo].[WsTokenPreventivo] CHECK CONSTRAINT [FK_WsTokenPreventivo_WsUser]
GO

