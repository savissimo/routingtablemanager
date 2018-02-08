using Desktop.ViewModels;
using Desktop.Views;
using RoutingTableManager;
using System.IO;
using System.Windows;

namespace Desktop
{
	/// <summary>
	/// Logica di interazione per App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			if (e.Args.Length > 0)
			{
				if (File.Exists(e.Args[0]))
				{
					RoutingTable routingTable = new RoutingTable();
					routingTable.LoadFromXml(e.Args[0]);
					routingTable.ApplyToSystem();
					MessageBox.Show("All routes applied.");
				}

				Application.Current.Shutdown();
			}

			MainViewModel mvm = new MainViewModel();
			MainWindow mv = new MainWindow();
			mvm.BindView(mv);
			mv.Show();
		}
	}
}
