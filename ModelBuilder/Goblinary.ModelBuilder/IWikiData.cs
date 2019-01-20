namespace Goblinary.ModelBuilder
{
	using System;
	using System.Data;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	internal interface IWikiData
	{
		bool IsLoaded { get; set; }
		XmlWriteMode WriteMode { get; }
		XmlReadMode ReadMode { get; }
		void Import();
	}

	internal static class IWikiDataExtensions
	{
		public static string XGetFileName(this IWikiData instance)
		{
			return string.Format("DataSets_{0}.xml", ((DataSet)instance).DataSetName);
		}

		public static void XRead(this IWikiData instance)
		{
			((DataSet)instance).Clear();
			instance.IsLoaded = false;
			string fileName = instance.XGetFileName();
			if (File.Exists(fileName))
			{
				((DataSet)instance).ReadXml(fileName, instance.ReadMode);
				instance.IsLoaded = true;
			}
		}

		public static void XSave(this IWikiData instance)
		{
			((DataSet)instance).WriteXml(instance.XGetFileName(), instance.WriteMode);
		}
	}
}
