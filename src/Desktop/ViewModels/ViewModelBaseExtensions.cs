using GalaSoft.MvvmLight;
using System.Windows.Controls;

namespace Desktop.ViewModels
{
	static class ViewModelBaseExtensions
	{
		public static void BindView(this ViewModelBase viewModel, Control i_control)
		{
			i_control.DataContext = viewModel;
		}
	}
}
