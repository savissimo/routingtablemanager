using System;
using System.Net;
using System.Xml.Serialization;

namespace RoutingTableManager
{
    public class Route : IEquatable<Route>
    {
		[XmlElement]
		public Host Destination { get; set; } = null;
		[XmlElement]
		public Host Gateway { get; set; } = null;
		[XmlElement]
		public int Interface { get; set; } = 0;

		private IPAddress m_mask = IPAddress.Parse("255.255.255.255");
		[XmlElement]
		public string Mask
		{
			get { return m_mask.ToString(); }
			set
			{
				m_mask = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
			}
		}

		public Route()
		{
		}

		public Route(IPAddress i_destinationIPAddress = null, string i_destinationHostName = null,
			IPAddress i_gatewayIPAddress = null, string i_gatewayHostName = null,
			int i_interface = 0)
			: this()
		{
			Destination = new Host(i_destinationIPAddress, i_destinationHostName);
			Gateway = new Host(i_gatewayIPAddress, i_gatewayHostName);
			Interface = i_interface;
		}

		internal void Merge(Route i_other, bool i_isAuthoritative)
		{
			if (i_other.Destination != Destination)
			{
				throw new InvalidOperationException("Cannot merge two Routes with different Desinations.");
			}

			if (Gateway == null || i_isAuthoritative)
			{
				Gateway = i_other.Gateway;
			}
			if (Interface == 0 || i_isAuthoritative)
			{
				Interface = i_other.Interface;
			}
		}

		public bool Equals(Route i_other)
		{
			return i_other.Destination == Destination;
		}

		private void ComputeIPAddress(bool i_mustRecompute = false)
		{
			Destination.ComputeIPAddress(i_mustRecompute);
			Gateway.ComputeIPAddress(i_mustRecompute);
		}

		internal void ApplyToSystem()
		{
			ComputeIPAddress();
			if (Destination.IPAddress != null && Gateway.IPAddress != null)
			{
				DeleteFromSystem();
				string command = string.Format("route add {0} {1} mask {3} if {2}", Destination.IPAddress, Gateway.IPAddress, Interface, Mask);
				Shell.RunCommand(command, true);
			}
		}

		internal void DeleteFromSystem()
		{
			if (Destination.IPAddress != null && Gateway.IPAddress != null)
			{
				string command = string.Format("route delete {0}", Destination.IPAddress);
				Shell.RunCommand(command, true);
			}
		}

		internal void UpdateDNSResolvedNames()
		{
			ComputeIPAddress(true);
		}
	}
}
