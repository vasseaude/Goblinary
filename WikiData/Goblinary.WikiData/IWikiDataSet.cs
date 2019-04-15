namespace Goblinary.WikiData
{
	using System.Data;
	using System.IO;

	public interface IWikiDataSet
	{
		bool IsLoaded { get; set; }
		XmlWriteMode WriteMode { get; }
		void Import();
	}

	public static class IWikiDataSetExtensions
	{
		public static string XGetFileName(this IWikiDataSet instance)
		{
			return string.Format("DataSets_{0}.xml", ((DataSet)instance).DataSetName);
		}

		public static void XRead(this IWikiDataSet instance)
		{
			((DataSet)instance).Clear();
			instance.IsLoaded = false;
			string fileName = instance.XGetFileName();
			if (File.Exists(fileName))
			{
				((DataSet)instance).ReadXml(fileName);
				instance.IsLoaded = true;
			}
		}

		public static void XSave(this IWikiDataSet instance)
		{
			((DataSet)instance).WriteXml(instance.XGetFileName(), instance.WriteMode);
		}
	}
}
