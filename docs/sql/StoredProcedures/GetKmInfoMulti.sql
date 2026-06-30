/****** Object:  StoredProcedure [dbo].[GetKmInfoMulti]    Script Date: 30/06/2026 14:30:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[GetKmInfoMulti]
	@KmIDMulti NVARCHAR(100) = null
AS
BEGIN

	SET NOCOUNT ON;

	SELECT * FROM (
		SELECT 
			LK.[ListinoKmID] as KmID   
			,CASE WHEN LK.Km = -1 THEN 'Illimitati' ELSE CAST(LK.[Km] as VARCHAR(30)) END as Descrizione
			,LK.Ordinamento as OrdinamentoKM
			,LG.Ordinamento as OrdinamentoGG
			,LG.Descrizione as Giorni
			,LG.ListinoID
			,LG.CodiceCategoria
			,CASE WHEN LK.Km = -1 THEN 0 ELSE LK.[Km] END as KmValue
		FROM [dbo].[ListinoKm] LK WITH (NOLOCK)
		INNER JOIN [dbo].[ListinoGiorni] LG WITH (NOLOCK) ON LK.ListinoGiorniID = LG.ListinoGiorniID

		UNION

		SELECT 
				[ListinoGiorniID] * -1      
				,CAST([FasciaStart] AS VARCHAR(10)) + '-' + CAST([FasciaEnd] AS VARCHAR(10)) as Descrizione
				,(Ordinamento + 40) as OrdinamentoKM
				,Ordinamento as OrdinamentoGG
				,Descrizione as Giorni
				,ListinoID
				,CodiceCategoria
				,([FasciaStart] + [FasciaEnd]) / 2 as KmValue
		FROM [dbo].[ListinoGiorni] WITH (NOLOCK)
	) KM
	WHERE @KmIDMulti IS NULL OR KM.KmID IN (SELECT Name FROM dbo.splitstring(@KmIDMulti))
END
GO

