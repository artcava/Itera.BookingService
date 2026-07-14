/****** Object:  View [dbo].[VwTipologiaFranchigiaListino]    Script Date: 14/07/2026 16:34:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VwTipologiaFranchigiaListino]
AS
	SELECT 
		LFT.ListinoID				AS [ListinoID]
		,TF.TipologiaFranchigiaID	AS [TipologiaFranchigia]
		,TF.Descrizione				AS [TipologiaFranchigiaDescrizione]
		,TF.Priorita				AS [TipologiaFranchigiaPriorita]
		,TF.Note					AS [TipologiaFranchigiaNote]
		,TF.TipologiaVoceFatturaID  AS [TipologiaVoceFatturaID]
		,LFT.StatoID				AS [StatoID]
		,SE.Descrizione				AS [StatoDescr]
		,LFT.StatoInclusione		AS [StatoInclusione]
		,TFC.Codice					AS [TipologiaFranchigiaCategoriaCodice]
	FROM ListinoFranchigiaTipologia LFT
		INNER JOIN TipologiaFranchigia TF ON LFT.TipologiaFranchigiaID = TF.TipologiaFranchigiaID
		LEFT JOIN TipologiaFranchigiaCategoria TFC ON TFC.TipologiaFranchigiaCategoriaID = TF.TipologiaFranchigiaCategoriaID
		INNER JOIN StatoElemento SE ON SE.Tipologia = 'GENERIC' AND LFT.StatoID = SE.Codice
	--WHERE LFT.StatoID = 1
GO

