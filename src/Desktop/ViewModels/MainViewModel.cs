using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RoutingTableManager;
using System.Collections.ObjectModel;
using System.Net;
using System;
using Desktop.Controls;
using Microsoft.Win32;
using System.Windows;

namespace Desktop.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		public ObservableCollection<Route> Routes { get { return m_routes; } }
        public ObservableCollection<Interface> AvailableInterfaces { get { return m_availableInterfaces; } }

		private RoutingTable m_routingTable = null;
		private ObservableCollection<Route> m_routes = null;

        private ObservableCollection<Interface> m_availableInterfaces = null;

        public RelayCommand LoadRoutingTableFromSystemCommand { get; set; }
		public RelayCommand LoadRoutingTableFromFileCommand { get; set; }
		public RelayCommand SaveRoutingTableToFileCommand { get; set; }
		public RelayCommand UpdateDNSResolvedNamesCommand { get; set; }
		public RelayCommand ApplyRoutingTableCommand { get; set; }

		private PersistentOpenFileDialog m_ofd = new PersistentOpenFileDialog();

		public MainViewModel()
		{
			if (IsInDesignMode)
			{
				m_routingTable = new RoutingTable();
				m_routingTable.Routes.Add(new Route(IPAddress.Parse("1.2.3.4")));
				m_routingTable.Routes.Add(new Route(IPAddress.Parse("5.6.7.8"), "example.test"));
				m_routingTable.Routes.Add(new Route());

				m_availableInterfaces = new ObservableCollection<Interface>();
				m_availableInterfaces.Add(new Interface { Index = 1, Metric = 10, Name = "Prima", Status = Interface.InterfaceStatus.Disconnected, MTU = 1500 });
				m_availableInterfaces.Add(new Interface { Index = 2, Metric = 20, Name = "Seconda", Status = Interface.InterfaceStatus.Connected, MTU = 1500 });
				m_availableInterfaces.Add(new Interface { Index = 3, Metric = 50, Name = "Terza", Status = Interface.InterfaceStatus.Disconnected, MTU = 1500 });
			}
			else
			{
				m_routingTable = new RoutingTable();
                m_availableInterfaces = new ObservableCollection<Interface>(Interface.GetAvailableInterfaces());
			}

			UpdateRoutes();

			LoadRoutingTableFromFileCommand = new RelayCommand(LoadRoutingTableFromFile);
			SaveRoutingTableToFileCommand = new RelayCommand(SaveRoutingTableToFile);
			ApplyRoutingTableCommand = new RelayCommand(ApplyRoutingTable);
			UpdateDNSResolvedNamesCommand = new RelayCommand(UpdateDNSResolvedNames);
		}

		private void UpdateRoutes()
		{
			m_routes = new ObservableCollection<Route>(m_routingTable.Routes);
			RaisePropertyChanged(nameof(Routes));
		}

		private void LoadRoutingTableFromFile()
		{
			if (m_ofd.ShowDialog() == true)
			{
				m_routingTable.LoadFromXml(m_ofd.FileName);
				UpdateRoutes();
			}
		}

		private void SaveRoutingTableToFile()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.AddExtension = true;
			sfd.DefaultExt = "rtxml";
			if (sfd.ShowDialog() == true)
			{
				m_routingTable.SaveToXml(sfd.FileName);
			}
		}

		private void ApplyRoutingTable()
		{
			if (m_routingTable != null)
			{
				m_routingTable.ApplyToSystem();
				MessageBox.Show("All routes applied.");
			}
		}

		private void UpdateDNSResolvedNames()
		{
			if (m_routingTable != null)
			{
				m_routingTable.UpdateDNSResolvedNames();
				UpdateRoutes();
				MessageBox.Show("All Host Names have been looked up.");
			}
		}
	}
}
