using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace WeSplitApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Trip a;

		public MainWindow()
		{
			InitializeComponent();
			a = new Trip();
			var b = new List<Trip.TripImage>();
			var c = new Trip.TripImage();
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			c.ImagePath = "/Images/about.png";
			b.Add(c);
			a.ImagesList = b;
			var d = new List<Member>();
			var e = new Member();
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			e.MemberName = "abc";
			d.Add(e);
			a.MembersList = d;
			a.Avatar = "/Images/about.png";
			a.Status = "Kết thúc";
			a.TripDestination = "Sài Gòn";
			a.TripName = "Chơi Sài Gòn";
			DetailTripGrid.DataContext = a;
		}

		private void PieChart_Loaded(object sender, RoutedEventArgs e)
		{
			abc.Series = new SeriesCollection  {
				new PieSeries{
			Values = new ChartValues<decimal> { 9 },
			Title = "1"
			},
			new PieSeries{
			Values = new ChartValues<decimal> { 1 },
			Title = "2"
			},
			new PieSeries{
			Values = new ChartValues<decimal> { 1 },
			Title = "3"
			},
			new PieSeries{
			Values = new ChartValues<decimal> { 3 },
			Title = "4"
			}
		};
		}
    }
}
