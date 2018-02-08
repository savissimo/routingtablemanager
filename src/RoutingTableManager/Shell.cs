using System.Diagnostics;

namespace RoutingTableManager
{
	class Shell
	{
		public static string RunCommand(string i_command, bool i_runAsAdministrator = false)
		{
			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = "cmd.exe";
			p.StartInfo.Arguments = "/c " + i_command;
			p.StartInfo.CreateNoWindow = true;
			if (i_runAsAdministrator)
			{
				p.StartInfo.Verb = "runas";
			}
			p.Start();
			string output = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			return output;
		}
	}
}
