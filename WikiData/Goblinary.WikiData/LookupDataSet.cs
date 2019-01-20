namespace Goblinary.WikiData
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Goblinary.Common;

	public partial class LookupDataSet : IWikiDataSet
	{
		private readonly string spreadsheetID = "11AQ0GcchezxiXRtFLVAT3BOCudR_Ie5XNVvENacDTvU";

		public bool IsLoaded { get; set; }
		public XmlWriteMode WriteMode { get { return XmlWriteMode.IgnoreSchema; } }

		public void Import()
		{
			this.Clear();
			this.IsLoaded = false;
			try
			{
				new SpreadsheetReader().FillDataSet(this, this.spreadsheetID);
			}
			catch (Exception ex)
			{
				var Errors = this.XGetErrors();
				throw ex;
			}
			this.IsLoaded = true;
			this.XSave();
		}
	}
}