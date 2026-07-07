/****** Object:  UserDefinedFunction [dbo].[GetAccessoriDettaglio]    Script Date: 06/07/2026 11:51:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO














CREATE FUNCTION [dbo].[GetAccessoriDettaglio]
(    
    -- Add the parameters for the function here
	@filialeID int,
	@segmentoMulti varchar(1000),
    @brandID smallint = NULL
)
RETURNS TABLE 
AS
RETURN 
(   
	SELECT 
	   VW.AccessorioTipologiaID
      ,AccessorioCategoriaID
      ,BrandID
      ,Codice
      ,DataInizioValidita
      ,DataFineValidita
      ,ImportoForzato
      ,IvaID
      ,InseribileFiliale
      ,ModificabileFiliale
      ,OneriAptRw
	  ,PercentualeAccessorio
      ,PercentualeOpen
	  ,PercentualeShort
      ,Obbligatorio
	  ,Preselezionato
	  ,MomentoVendibilita
	  ,AccessorioTipologiaIDPenale
      ,PrepagamentoContactCenter
      ,PrepagamentoWeb
      ,StatoID
      ,TipologiaVoceFatturaID
      ,VendibilitaContactCenter
      ,VendibilitaFiliale
      ,VendibilitaWeb
      ,DescrizioneBrand
      ,CodiceBrand
      ,DescrizioneTipologiaVoceFattura
      ,DescrizioneFatturazioneTipologiaVoceFattura
      ,CodArticoloFatturazione
      ,CodArticoloFatturazioneSostitutivo
      ,Attiva
      ,DescrizioneIva
      ,PercentualeIva
      ,DescrizioneCategoria
      ,CodiceCategoria
	  ,StatoIDCategoria
	  ,ASS.CodiceSegmento
  FROM [dbo].[VwAccessori] VW
  INNER JOIN [dbo].[AccessorioFiliale] AF ON VW.AccessorioTipologiaID = AF.AccessorioTipologiaID AND AF.FilialeID = @filialeID
  INNER JOIN [dbo].[AccessorioSegmento] ASS on VW.AccessorioTipologiaID = ASS.AccessorioTipologiaID
  WHERE (VW.BrandID IS NULL OR (@brandID IS NULL OR VW.BrandID = @brandID))	
		AND (@segmentoMulti IS NULL OR ASS.CodiceSegmento IN (SELECT Name FROM dbo.splitstring(@segmentoMulti)))
)
GO

