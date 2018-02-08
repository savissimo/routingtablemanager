using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RoutingTableManager
{
    public class Interface
    {
        public enum InterfaceStatus
        {
            Disconnected, 
            Connected
        }

        public int Index { get; set; }
        public int Metric { get; set; }
        public long MTU { get; set; }
        public InterfaceStatus Status { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public static List<Interface> GetAvailableInterfaces()
        {
            string output = Shell.RunCommand("netsh int ipv4 show interfaces", false);

            List<Interface> interfaces = new List<Interface>();

            string[] lines = output.Split("\n".ToCharArray());
            Regex lineFormat = new Regex(@"\s*(?<idx>[0-9]+)\s+(?<metric>[0-9]+)\s+(?<mtu>[0-9]+)\s+(?<status>[a-z]+)\s+(?<name>.+)\r");
            foreach (string line in lines)
            {
                Match match = lineFormat.Match(line);
                if (match.Success)
                {
                    Interface retval = new Interface();
                    retval.Index = int.Parse(match.Groups["idx"].Value);
                    retval.Metric = int.Parse(match.Groups["metric"].Value);
                    retval.MTU = long.Parse(match.Groups["mtu"].Value);
                    switch(match.Groups["status"].Value)
					{
						case "disconnected": retval.Status = InterfaceStatus.Disconnected; break;
						case "connected": retval.Status = InterfaceStatus.Connected; break;
					}
					retval.Name = match.Groups["name"].Value;
					interfaces.Add(retval);
                }
            }

            return interfaces;
        }
    }
}
