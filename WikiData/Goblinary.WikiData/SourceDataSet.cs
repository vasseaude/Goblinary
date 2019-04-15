namespace Goblinary.WikiData
{
	using System;
	using System.Data;
	using Common;

	public partial class SourceDataSet : IWikiDataSet
	{
		public bool IsLoaded { get; set; }
		public XmlWriteMode WriteMode => XmlWriteMode.WriteSchema;
	    public LookupDataSet LookupDataSet { get; set; }

		//private string SpreadsheetId => LookupDataSet.Dictionary.FindByKey("Copy of PFO Wiki - Official Data").Value;
	    private string SpreadsheetId = "1UsN8eNlnD7iM_XcFYRvVJJDGTM-0ShcOTQyGcgrxqA0";

        public void Import()
		{
			if (!LookupDataSet.IsLoaded)
				LookupDataSet.XRead();
			Clear();
			IsLoaded = false;
			try
			{
				new SpreadsheetReader().FillDataSet(this, SpreadsheetId);
			}
			catch (Exception exception)
			{
                //what is the point of this
				var errors = this.XGetErrors();
				throw exception;
			}
			IsLoaded = true;
			this.XSave();
		}
	}
}