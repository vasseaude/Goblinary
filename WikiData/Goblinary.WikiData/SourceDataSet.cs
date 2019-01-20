namespace Goblinary.WikiData
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Goblinary.Common;

	public partial class SourceDataSet : IWikiDataSet
	{
		public bool IsLoaded { get; set; }
		public XmlWriteMode WriteMode { get { return XmlWriteMode.WriteSchema; } }
		public LookupDataSet LookupDataSet { get; set; }

		private string spreadsheetID
		{
			get
			{
				return this.LookupDataSet.Dictionary.FindByKey("Copy of PFO Wiki - Official Data").Value;
			}
		}

		public void Import()
		{
			if (!this.LookupDataSet.IsLoaded)
				this.LookupDataSet.XRead();
			this.Clear();
			this.IsLoaded = false;
			try
			{
				new SpreadsheetReader().FillDataSet(this, this.spreadsheetID);
			}
			catch (Exception ex)
			{
				var errors = this.XGetErrors();
				throw ex;
			}
			this.IsLoaded = true;
			this.XSave();
		}
	}
}