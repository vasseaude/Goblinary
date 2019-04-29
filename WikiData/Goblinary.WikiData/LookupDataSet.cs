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
			    Dictionary.ReadXml("Dictionary.xml");
			    SourceWorksheets.ReadXml("Worksheets.xml");
			    WeaponTypes.ReadXml("WeaponTypes.xml");
                Lookups.ReadXml("Lookups.xml");
                EffectLookups.ReadXml("EffectLookups.xml");
                EntityTypes.ReadXml("EntityTypes.xml");
                GearTypes.ReadXml("GearTypes.xml");
                Resources.ReadXml("Resources.xml");
			    ResourceTypes.ReadXml("ResourceTypes.xml");
                Salvages.ReadXml("Salvages.xml");
                Stocks.ReadXml("Stocks.xml");
                StockIngredients.ReadXml("StockIngredients.xml");
			    KeywordTypes.ReadXml("KeywordTypes.xml");

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