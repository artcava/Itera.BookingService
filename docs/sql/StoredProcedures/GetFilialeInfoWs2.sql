-- Stored Procedure originale: [dbo].[GetFilialeInfoWs2]
-- Comportamento replicato in: src/Itera.BookingService.Infrastructure/Branch/LegacyBranchInfoQueryService.cs
-- Query type EF Core: src/Itera.BookingService.Infrastructure/Persistence/Entities/Wave2CorrelatedQueryTypes.cs -> GetFilialeInfoWs2Result
-- Data migrazione: 2026-06-25
-- NOTA: questo file è mantenuto a scopo documentale. Non modificare.

/****** Object:  StoredProcedure [dbo].[GetFilialeInfoWs2]    Script Date: 25/06/2026 16:20:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[GetFilialeInfoWs2]
	@BrandID smallint = NULL,
	@FilialeID int = NULL,
	@LinguaID tinyint
AS
BEGIN

	SET NOCOUNT ON;

	SELECT F.[FilialeID]
		  ,(SELECT Valore FROM Testo T WHERE T.Chiave = FT.Descrizione AND T.LinguaID = @LinguaID) as Descrizione
		  ,F.[FranchiseID]
		  ,F.[Indirizzo]
		  ,F.[Cap]
		  ,F.[Citta]
		  ,F.[Telefono]
		  ,F.[Fax]
		  ,F.Email 
		  ,FC.Descrizione as DescrizioneFranchise
		  ,F.SiglaAutomobilistica
		  ,FC.CodiceAzienda
		  ,(SELECT Valore FROM Testo T WHERE T.Chiave = FT.OrariUfficio AND T.LinguaID = @LinguaID) as OrariUfficioDescrizione
		  ,(SELECT Valore FROM Testo T WHERE T.Chiave = FT.OrariConsegna AND T.LinguaID = @LinguaID) as OrariConsegnaDescrizione
		  ,F.CoordX
		  ,F.CoordY
		  ,F.SpnX
		  ,F.SpnY
		  ,F.Zoom
		  ,Stato
		  ,Parcheggio
		  ,RespCommerciale
		  ,RespAmministrazione		
		  ,KeyBox
          ,EsclusioneVal
		  ,FleetNonVendibile
		  ,FA.FilialeAreaID
		  ,FA.FilialeMacroAreaID
		  ,FCF.FilialeClassificazioneID as ClassificazioneID
		  ,FCF.Descrizione as ClassificazioneDescrizione
		  ,PV.Denominazione as Provincia
		  ,RG.Denominazione as Regione
	FROM Filiale F
		INNER JOIN Franchise FC ON F.FranchiseID = FC.FranchiseID
		INNER JOIN FilialeTesto as FT ON F.FilialeID = FT.FilialeID
		INNER JOIN FilialeArea FA ON FA.FilialeAreaID = F.FilialeAreaID
		LEFT JOIN FilialeClassificazione FCF ON FCF.FilialeClassificazioneID = F.FilialeClassificazioneID
		LEFT JOIN Provincia PV ON PV.SiglaAutomobilistica = F.SiglaAutomobilistica
		LEFT JOIN Regione RG ON PV.CodiceRegione = RG.CodiceRegione
		INNER JOIN FilialeBrand FB ON F.FilialeID = FB.FilialeID

	WHERE 1 = 1
		AND (@FilialeID IS NULL OR F.FilialeID = @FilialeID)
		AND (@BrandID IS NULL OR FB.BrandID = @BrandID)

END
