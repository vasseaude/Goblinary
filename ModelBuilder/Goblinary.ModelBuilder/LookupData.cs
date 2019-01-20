namespace Goblinary.ModelBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Goblinary.Common;

	public partial class LookupData : IWikiData
	{
		private readonly string spreadsheetID = "1Tpxg3yV-2A83BOuS3qA4XvRAA2OxexFoFeOzOtkNx60";

		public bool IsLoaded { get; set; }
		public XmlWriteMode WriteMode { get { return XmlWriteMode.IgnoreSchema; } }
		public XmlReadMode ReadMode { get { return XmlReadMode.IgnoreSchema; } }

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