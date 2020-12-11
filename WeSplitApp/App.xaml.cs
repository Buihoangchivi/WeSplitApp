using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WeSplitApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			//View.MainPage window = new View.MainPage();
			View.HomeWindow window = new View.HomeWindow();
			//UserViewModel VM = new UserViewModel();
			HomeViewModel homeViewModel = new HomeViewModel();
			window.DataContext = homeViewModel;
			window.Show();
		}
	}
}
