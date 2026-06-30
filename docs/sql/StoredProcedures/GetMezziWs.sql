/****** Object:  StoredProcedure [dbo].[GetMezziWs]    Script Date: 29/06/2026 17:12:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetMezziWs]
	@FleetMulti varchar(50) = NULL,
	@SegmentoMulti varchar(100) = NULL,
	@MezzoSpeciale bit = NULL,
	@GruppoID int = NULL
AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		MM.ModelloMezzoID,
		MA.Descrizione as Marca,
		MM.Descrizione as ModelloDescr,
		MM.NomeImmagine,
		MM.Cilindrata,
		AM.AlimentazioneModelloID,
		AM.Descrizione as AlimentazioneDescr,
		MM.Euro,
		MM.NumeroPosti,
		MM.NumeroPorte,
		MM.Autoradio,
		MM.AriaCondizionata,
		MM.Abs,
		MM.Airbag,
		MM.CapacitaSerbatoio,
		MM.Portata,
		MM.VolumeCarico,
		MM.AltezzaInterna,
		MM.LarghezzaInterna,
		MM.LunghezzaInterna,
		SM.Descrizione as SegmentoDescrizione,
		SM.CodiceSegmento,
		SM.ModelloMezzoIDErs,
		SM.IndexPricing,
		SMC.SegmentoModelloClasseID,
		SMC.Descrizione as SegmentoModelloClasseIDDescrizione,
		MM.AltezzaEsterna,
		MM.LarghezzaEsterna,
		MM.LunghezzaEsterna,
		MM.Passo,
		MM.LarghezzaPassaruote,
		MM.NumeroPostiCarrozzina,
		MM.NumeroPostiMobility,
		MM.PedanaSollevatriceDoppioBraccio,
		MM.DescrizioneMobilitySitoWeb_ITA,
		MM.DescrizioneMobilitySitoWeb_ENG
	FROM 
		ModelloMezzo AS MM 
		INNER JOIN SegmentoModello AS SM ON MM.CodiceSegmento = SM.CodiceSegmento
		INNER JOIN AlimentazioneModello AS AM ON MM.AlimentazioneModelloID = AM.AlimentazioneModelloID
		INNER JOIN Marca MA ON MM.MarcaID = MA.MarcaID
		INNER JOIN SegmentoModelloClasse SMC ON SMC.SegmentoModelloClasseID = SM.SegmentoModelloClasseID
		LEFT JOIN ModelloMezzoGruppo AS MMG ON MM.ModelloMezzoID = MMG.ModelloMezzoID
	WHERE 1 = 1
		AND (MM.VisibilitaSito IS NULL OR MM.VisibilitaSito = 1)
		AND (@FleetMulti IS NULL OR SM.FleetID IN (SELECT Name FROM dbo.splitstring(@FleetMulti)))
		AND ((@MezzoSpeciale IS NULL) OR (@MezzoSpeciale = 1 AND SM.CodiceSegmento IN (SELECT CodiceSegmento FROM SegmentoModelloMezzoSpeciale))
						OR (@MezzoSpeciale = 0 AND SM.CodiceSegmento NOT IN (SELECT CodiceSegmento FROM SegmentoModelloMezzoSpeciale)))
		AND (@GruppoID IS NULL OR MMG.GruppoID = @GruppoID)
		AND (@SegmentoMulti IS NULL OR SM.CodiceSegmento IN (SELECT Name FROM dbo.splitstring(@SegmentoMulti)))

	ORDER BY SM.Ordinamento ASC
	
				
END
GO

