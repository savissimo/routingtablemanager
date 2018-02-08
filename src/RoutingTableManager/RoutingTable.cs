using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace RoutingTableManager
{
	[XmlRoot]
	public class RoutingTable
	{
		[XmlElement("Route")]
		public List<Route> Routes { get; private set; } = new List<Route>();

		public RoutingTable()
		{
		}

		private void Merge(RoutingTable i_other, bool i_isAuthoritative)
		{
			foreach (Route r in i_other.Routes)
			{
				Route mergeable = Routes.Where(route => route == r).SingleOrDefault();
				if (mergeable == null)
				{
					Routes.Add(r);
				}
				else
				{
					mergeable.Merge(r, i_isAuthoritative);
				}
			}
		}

		public void LoadFromXml(string i_fileName)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(RoutingTable));
			using (FileStream fs = new FileStream(i_fileName, FileMode.OpenOrCreate))
			{
				RoutingTable result = serializer.Deserialize(fs) as RoutingTable;
				Merge(result, true);
				fs.Close();
			}
		}

		public void SaveToXml(string i_fileName)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(RoutingTable));
			using (FileStream fs = new FileStream(i_fileName, FileMode.OpenOrCreate))
			{
				serializer.Serialize(fs, this);
				fs.Close();
			}
		}

		public void ApplyToSystem()
		{
			foreach (Route r in Routes)
			{
				r.ApplyToSystem();
			}
		}

		public void UpdateDNSResolvedNames()
		{
			foreach (Route r in Routes)
			{
				r.UpdateDNSResolvedNames();
			}
		}
	}
}
