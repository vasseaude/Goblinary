﻿namespace Goblinary.ModelBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;

	using Goblinary.Common;

	public partial class StockData : IWikiData
	{
		public bool IsLoaded { get; set; }
		public XmlWriteMode WriteMode { get { return XmlWriteMode.IgnoreSchema; } }
		public XmlReadMode ReadMode { get { return XmlReadMode.IgnoreSchema; } }
		public LookupData LookupData { get; set; }

		private string spreadsheetID
		{
			get
			{
				return this.LookupData.Dictionary.FindByKey("StockData").Value;
			}
		}

		public void Import()
		{
			if (!this.LookupData.IsLoaded)
			{
				this.LookupData.XRead();
			}
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