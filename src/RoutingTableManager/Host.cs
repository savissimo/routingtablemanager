using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace RoutingTableManager
{
	public class Host : IEquatable<Host>
	{
		[XmlIgnore]
		public IPAddress IPAddress { get; set; } = null;
		[XmlAttribute("hostname")]
		public string HostName { get; set; } = null;

		[XmlAttribute("ipaddress")]
		public string IPAddressForXml
		{
			get { return IPAddress != null ? IPAddress.ToString() : null; }
			set
			{
				IPAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
			}
		}

		[XmlIgnore]
		public string Label
		{
			get
			{
				if (HostName != null)
				{
					if (IPAddress != null)
					{
						return string.Format("{0} [{1}]", HostName, IPAddress);
					}
					return HostName;
				}
				else if (IPAddress != null)
				{
					return IPAddress.ToString();
				}
				return "---";
			}
		}

		[XmlIgnore]
		public string Value
		{
			get
			{
				if (HostName != null)
				{
					return HostName;
				}
				else if (IPAddress != null)
				{
					return IPAddress.ToString();
				}
				return null;
			}
			set
			{
				IPAddress parsedIPAddress = null;
				if (IPAddress.TryParse(value, out parsedIPAddress))
				{
					IPAddress = parsedIPAddress;
					HostName = null;
				}
				else
				{
					HostName = value;
					IPAddress = null;
				}
			}
		}

		public Host()
		{
		}

		public Host(IPAddress i_IPAddress = null, string i_hostName = null)
			: this()
		{
			IPAddress = i_IPAddress;
			HostName = i_hostName;
		}

		public override string ToString()
		{
			return Label;
		}

		public bool Equals(Host i_other)
		{
			if (HostName != null && i_other.HostName != null)
			{
				return HostName == i_other.HostName;
			}
			if (IPAddress != null && HostName != null)
			{
				return IPAddress == i_other.IPAddress;
			}
			return false;
		}

		internal void ComputeIPAddress(bool i_mustRecompute = false)
		{
			if (IPAddress == null || i_mustRecompute)
			{
				if (HostName == null)
				{
					return;
				}

				string cmdResult = Shell.RunCommand(string.Format("ping -a -n 1 {0}", HostName));
				Regex regex = new Regex(string.Format("(?<hostname>{0})? \\[(?<ip>[0-9\\.]{{7,}})\\]", HostName));
				Match foundIP = regex.Match(cmdResult);
				if (foundIP.Success)
				{
					IPAddress = IPAddress.Parse(foundIP.Groups["ip"].Value);
				}
			}
		}
	}
}
