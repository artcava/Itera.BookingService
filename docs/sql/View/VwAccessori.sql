/****** Object:  View [dbo].[VwAccessori]    Script Date: 06/07/2026 11:55:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO












CREATE VIEW [dbo].[VwAccessori]
AS
SELECT     ACT.AccessorioTipologiaID,
           ACT.AccessorioCategoriaID, 
		   ACT.BrandID,
		   ACT.Codice,
		   ACT.DataInizioValidita,
		   ACT.DataFineValidita,
		   ACT.ImportoForzato,
		   TVF.IvaID,
		   ACT.InseribileFiliale,
		   ACT.ModificabileFiliale,
		   ACT.OneriAptRw,  
		   ACT.PercentualeAccessorio,
           ACT.PercentualeOpen, 
		   ACT.PercentualeShort, 
		   ACT.Obbligatorio, 		   
		   ACT.PrepagamentoContactCenter,
		   ACT.PrepagamentoWeb,
		   ACT.StatoID,
		   ACT.TipologiaVoceFatturaID,
		   ACT.VendibilitaContactCenter,
		   ACT.VendibilitaFiliale,
		   ACT.VendibilitaWeb,
		   ACT.ImportoPenale,
		   ACT.Preselezionato,
		   ACT.MomentoVendibilita,
		   ACT.AccessorioTipologiaIDPenale,
		   BR.Descrizione as DescrizioneBrand,
		   BR.CodiceBrand as CodiceBrand, 
		   TVF.Descrizione as DescrizioneTipologiaVoceFattura,
		   TVF.DescrizioneFatturazione as DescrizioneFatturazioneTipologiaVoceFattura, 		   
		   TVF.CodArticoloFatturazione, 
		   TVF.CodArticoloFatturazioneSostitutivo,		   
		   TVF.Attiva,		   
		   IVA.Descrizione as DescrizioneIva,
		   IVA.Percentuale as PercentualeIva,
		   ACC.Descrizione as DescrizioneCategoria,
		   ACC.Codice as CodiceCategoria,
		   ACC.StatoID as StatoIDCategoria
FROM       dbo.AccessorioTipologia ACT WITH (NOLOCK)
           INNER JOIN dbo.AccessorioCategoria ACC WITH (NOLOCK) ON ACC.AccessorioCategoriaID = ACT.AccessorioCategoriaID
		   INNER JOIN dbo.TipologiaVoceFattura TVF WITH (NOLOCK) ON TVF.TipologiaVoceFatturaID = ACT.TipologiaVoceFatturaID
		   LEFT OUTER JOIN dbo.Iva IVA WITH (NOLOCK) ON IVA.IvaID = ACT.IvaID
		   LEFT OUTER JOIN dbo.Brand BR WITH (NOLOCK) ON BR.BrandID = ACT.BrandID
GO

