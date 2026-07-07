/****** Object:  UserDefinedFunction [pricing].[GetTariffeRdvImporto]    Script Date: 07/07/2026 10:54:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [pricing].[GetTariffeRdvImporto]
(    
	 @listinoID					INT
	,@accessorioIDList			VARCHAR(MAX)
	,@dataValidita				DATETIME
	,@giorniNoleggio			INT
	,@brandIDTariffario			SMALLINT
	,@accordoCommercialeID		INT
)

RETURNS @tariffaTbl	TABLE
(
	 [TariffarioID]						INT				NOT NULL
	,[DescrizioneTariffario]			NVARCHAR(100)	NOT NULL
	,[AccessorioTipologiaID]			SMALLINT		NOT NULL
	,[DescrizioneAccessorio]			NVARCHAR(250)	NOT NULL
	,[TipologiaVoceFatturaID]			TINYINT
	,[IsNotaCredito]					BIT
	,[DataStart]						DATETIME		NOT NULL
	,[DataEnd]							DATETIME		NOT NULL
	,[BreakEven]						SMALLINT		NOT NULL
	,[MinGiorniApplicabilita]			SMALLINT		NOT NULL
	,[MaxGiorniApplicabilita]			SMALLINT		NOT NULL
	,[Percentuale]						DECIMAL	
	,[ImportoFisso]						MONEY			NOT NULL
	,[TipoImporto]						VARCHAR(3)		NOT NULL
	,[ImportoGiornoExtra]				MONEY		
	,[ImportoMinAddebitabile]			MONEY
	,[ImportoMaxAddebitabile]			MONEY
	,[MaxGiorniAddebitabili]			SMALLINT
	,[Tolleranza]						DECIMAL(5,2)
	,[StatoInclusione]					VARCHAR(3)
	,[Incasso]							VARCHAR(3)
	,[StatoID]							TINYINT			NOT NULL
	,[MomentoVendibilita]				CHAR(2)
)
AS
BEGIN
	DECLARE @tariffarioID INT = NULL

	SET @tariffarioID =
	(
		SELECT TariffarioID
		FROM AccordoCommercialeListino
		WHERE AccordoCommercialeID = @accordoCommercialeID
		AND ListinoID = @listinoID
		AND PeriodoValiditaDa <= @dataValidita AND PeriodoValiditaA >=  CONVERT(DATE, @dataValidita)
	)

	SET @tariffarioID =
	(
		SELECT 
		CASE WHEN @tariffarioID IS NULL THEN li.[TariffarioID]
			 ELSE @tariffarioID END
		FROM Listino	AS li
		INNER JOIN [pricing].[Tariffario] AS ta ON (li.[TariffarioID] = ta.[TariffarioID])
		WHERE ListinoID = @listinoID
		  AND (ta.[BrandID] IS NULL OR ta.[BrandID] = COALESCE(@brandIDTariffario, ta.[BrandID]))
	)

	DECLARE @accessorioIDTbl TABLE ([Name] SMALLINT)
	INSERT INTO @accessorioIDTbl
	SELECT Name FROM dbo.splitstring(@accessorioIDList)

	;WITH cte AS
	(
		SELECT PTR.[AccessorioTipologiaID]	AS [AccessorioTipologiaID]
			  ,PTR.[TariffaRdvID]			AS [TariffaRdvID]
			  ,PTR.[BreakEven]				AS [BreakEven]
			  ,PTR.[MinGiorniApplicabilita]	AS [MinGiorniApplicabilita]
			  ,PTR.[MaxGiorniApplicabilita]	AS [MaxGiorniApplicabilita]
			  ,PTR.[TipoImporto]			AS [TipoImporto]
		FROM [pricing].TariffaRdv				AS PTR
		INNER JOIN @accessorioIDTbl				AS ACC	ON ACC.[Name] = PTR.[AccessorioTipologiaID]
		WHERE PTR.[TariffarioID] = @tariffarioID
		  AND PTR.[ImportoFisso] IS NOT NULL
		  AND PTR.[ImportoFisso] >= 0
		  AND PTR.[DataStart]	 <= cast(@dataValidita as date)
		  AND PTR.[DataEnd]		 >= cast(@dataValidita as date)
		  AND PTR.[BreakEven]	 >= @giorniNoleggio

	),
	cte2 AS
	(
		SELECT [TariffaRdvID]		AS [TariffaRdvID]
		FROM
		(
			SELECT ROW_NUMBER() OVER (PARTITION BY [AccessorioTipologiaID] ORDER BY [BreakEven]) AS [Row]
				  ,[TariffaRdvID]
				  ,[MinGiorniApplicabilita]
				  ,[MaxGiorniApplicabilita]
				  ,[TipoImporto]
			FROM cte
		) AS cte2
		WHERE cte2.[Row] = 1
		  AND cte2.[MinGiorniApplicabilita] <= @giorniNoleggio
		  AND cte2.[MaxGiorniApplicabilita] >= CASE WHEN [TipoImporto] = 'U'
												THEN @giorniNoleggio
												ELSE cte2.[MaxGiorniApplicabilita]
												END
	)

	INSERT INTO @tariffaTbl
	SELECT PTR.[TariffarioID]				AS [TariffarioID]
		  ,TRF.[Descrizione]				AS [DescrizioneTariffario]
		  ,PTR.[AccessorioTipologiaID]		AS [AccessorioTipologiaID]
		  ,TVF.[Descrizione]				AS [DescrizioneAccessorio]
		  ,TVF.[TipologiaVoceFatturaID]		AS [TipologiaVoceFatturaID]
		  ,TVF.[IsNotaCredito]				AS [IsNotaCredito]
		  ,PTR.[DataStart]					AS [DataStart]
		  ,PTR.[DataEnd]					AS [DataEnd]
		  ,PTR.[BreakEven]					AS [BreakEven]
		  ,PTR.[MinGiorniApplicabilita]		AS [MinGiorniApplicabilita]
		  ,PTR.[MaxGiorniApplicabilita]		AS [MaxGiorniApplicabilita]
		  ,PTR.[Percentuale]				AS [Percentuale]
		  ,PTR.[ImportoFisso]				AS [ImportoFisso]
		  ,PTR.[TipoImporto]				AS [TipoImporto]
		  ,PTR.[ImportoGiornoExtra]			AS [ImportoGiornoExtra]
		  ,PTR.[ImportoMinAddebitabile]		AS [ImportoMinAddebitabile]
		  ,PTR.[ImportoMaxAddebitabile]		AS [ImportoMaxAddebitabile]
		  ,PTR.[MaxGiorniAddebitabili]		AS [MaxGiorniAddebitabili]
		  ,PTR.[Tolleranza]					AS [Tolleranza]
		  ,PTR.[StatoInclusione]			AS [StatoInclusione]
		  ,PTR.[Incasso]					AS [Incasso]
		  ,PTR.[StatoID]					AS [StatoID]
		  ,ACC.[MomentoVendibilita]			AS [MomentoVendibilita]
	FROM cte2
	INNER JOIN [pricing].[TariffaRdv]		AS PTR	ON PTR.[TariffaRdvID] = cte2.[TariffaRdvID]
	INNER JOIN [pricing].[Tariffario]		AS TRF	ON TRF.[TariffarioID] = PTR.[TariffarioID]
	INNER JOIN [AccessorioTipologia]		AS ACC	ON ACC.[AccessorioTipologiaID] = PTR.[AccessorioTipologiaID]
	INNER JOIN [TipologiaVoceFattura]		AS TVF	ON TVF.[TipologiaVoceFatturaID] = ACC.[TipologiaVoceFatturaID]

	RETURN
	
END
GO

