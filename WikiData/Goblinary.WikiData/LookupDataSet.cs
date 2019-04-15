namespace Goblinary.WikiData
{
	using System;
	using System.Data;
	using Common;

	public partial class LookupDataSet : IWikiDataSet
	{
		private readonly string spreadsheetID = "11AQ0GcchezxiXRtFLVAT3BOCudR_Ie5XNVvENacDTvU";

		public bool IsLoaded { get; set; }
		public XmlWriteMode WriteMode => XmlWriteMode.IgnoreSchema;

	    public void Import()
		{
			Clear();
			IsLoaded = false;
			try
			{
				//new SpreadsheetReader().FillDataSet(this, spreadsheetID);
			    this.Dictionary.ReadXml("Dictionary.xml");
			    this.SourceWorksheets.ReadXml("Worksheets.xml");
			    this.WeaponTypes.ReadXml("WeaponTypes.xml");
                this.Lookups.ReadXml("Lookups.xml");
                this.EffectLookups.ReadXml("EffectLookups.xml");
                this.EntityTypes.ReadXml("EntityTypes.xml");
                this.GearTypes.ReadXml("GearTypes.xml");
                this.Resources.ReadXml("Resources.xml");
			    this.ResourceTypes.ReadXml("ResourceTypes.xml");
                this.Salvages.ReadXml("Salvages.xml");
                this.Stocks.ReadXml("Stocks.xml");
                this.StockIngredients.ReadXml("StockIngredients.xml");
			    this.KeywordTypes.ReadXml("KeywordTypes.xml");

			}

            catch (Exception ex)
			{
				var Errors = this.XGetErrors();
				throw ex;
			}
			IsLoaded = true;
			this.XSave();
		}
	}
}