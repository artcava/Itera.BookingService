/****** Object:  View [dbo].[VwListinoFranchigia]    Script Date: 14/07/2026 16:32:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[VwListinoFranchigia]
AS
	SELECT
	   LF.[ListinoFranchigiaID]
      ,LF.[DurataID]
      ,LF.[CodiceSegmento]
      ,LF.[PenaleRisarcitoriaRCAuto]
      ,LF.[PenaleRisarcitoriaDanni]
      ,LF.[PenaleRisarcitoriaIncendioFurto]
      ,LF.[CostoCoperturaExtra]
      ,LF.[CostoCoperturaExtraSoglia]
      ,LF.[PenaleRisarcitoriaDanniRidotta]
      ,LF.[PenaleRisarcitoriaIncendioFurtoRidotta]
      ,LF.[ValidaDal]
      ,LF.[ValidaAl]
      ,LF.[ListinoID]
	  ,LF.[SubCodice]
	  ,LF.[BreakEven]
	  ,LF.[MinGiorniApplicabilita] 
	  ,LF.[MaxGiorniApplicabilita] 
	  ,LF.[TipoImporto] 
	  ,LF.[ImportoGiornoExtra]
      ,VWTFL.[TipologiaFranchigia]
      ,VWTFL.[TipologiaFranchigiaDescrizione]
	  ,VWTFL.[TipologiaFranchigiaPriorita]
	  ,VWTFL.[TipologiaFranchigiaNote]	  
	  ,VWTFL.[TipologiaVoceFatturaID] 
      ,VWTFL.[StatoID]
      ,VWTFL.[StatoDescr]
	  ,VWTFL.[StatoInclusione]
	  ,VWTFL.[TipologiaFranchigiaCategoriaCodice]
	FROM ListinoFranchigia LF
	INNER JOIN VwTipologiaFranchigiaListino VWTFL
		ON LF.ListinoID = VWTFL.ListinoID AND LF.TipologiaFranchigiaID = VWTFL.TipologiaFranchigia	
	WHERE VWTFL.[TipologiaVoceFatturaID]  IS NOT NULL
GO

