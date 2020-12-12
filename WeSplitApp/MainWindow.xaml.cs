using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WeSplitApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//---------------------------------------- Khai báo các biến toàn cục --------------------------------------------//

		public event PropertyChangedEventHandler PropertyChanged;
		private Button clickedControlButton, clickedTypeButton;
		private List<Trip> TripInfoList = new List<Trip>();		//Danh sách thông tin tất cả các chuyến đi
		private CollectionView view;
		private List<Trip> TripOnScreen;						//Danh sách chuyến đi để hiện trên màn hình
		private List<ColorSetting> ListColor;
		private List<string> StageList;

		private bool isMinimizeMenu;
		private int TripPerPage = 12;           //Số chuyến đi mỗi trang
		private int _totalPage = 0;             //Tổng số trang
		public int TotalPage
		{
			get
			{
				return _totalPage;
			}
			set
			{
				_totalPage = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("TotalPage"));
				}
			}
		}
		private int _currentPage = 1;           //Trang hiện tại
		public int CurrentPage
		{
			get
			{
				return _currentPage;
			}
			set
			{
				_currentPage = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("CurrentPage"));
				}
			}
		}
		private string _totalItem = "0 item";   //Tổng số món ăn theo filter hiện tại
		public string TotalItem
		{
			get
			{
				return _totalItem;
			}
			set
			{
				_totalItem = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("TotalItem"));
				}
			}
		}

		private string _colorScheme = "";           //Màu nền hiện tại
		public string ColorScheme
		{
			get
			{
				return _colorScheme;
			}
			set
			{
				_colorScheme = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("ColorScheme"));
				}
			}
		}

		//---------------------------------------- Khai báo các class --------------------------------------------//

		//Class lưu trữ màu trong Color setting
		public class ColorSetting
		{
			public string Color { get; set; }
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//Đọc dữ liệu các món ăn từ data
			XmlSerializer xsFood = new XmlSerializer(typeof(List<Trip>));
			try
			{
				using (var reader = new StreamReader(@"Data\Trip.xml"))
				{
					TripInfoList = (List<Trip>)xsFood.Deserialize(reader);
				}
			}
			catch
			{
				TripInfoList = new List<Trip>();
			}

			TripOnScreen = TripInfoList;

			//Khởi tạo phân trang
			TotalPage = (TripInfoList.Count - 1) / TripPerPage + 1;
			TotalItem = TripInfoList.Count.ToString();
			if (TripInfoList.Count > 1)
			{
				TotalItem += " items";
			}
			else
			{
				TotalItem += " item";
			}
			UpdatePageButtonStatus();

			//TripInfoList = new List<Trip>
			//{
			//	new Trip
			//	{
			//		TripName = "abc",
			//		Location = "Quang Nam",
			//		Stage = 2,
			//		PrimaryImagePath = "Images\\1.jpg",
			//		IsFavorite = true,
			//		ImagesList = new List<string> { "Images\\2.jpg", "Images\\3.jpg" },
			//		MembersList = new List<Member>
			//		{
			//			new Member
			//			{
			//				MemberName = "Bui Van Vi",
			//				CostsList = new List<Cost>
			//				{
			//					new Cost
			//					{
			//						PaymentName = "Com D2",
			//						Charge = 20000
			//					},
			//					new Cost
			//					{
			//						PaymentName = "Sua chua nha dam",
			//						Charge = 7000
			//					}
			//				}
			//			},
			//			new Member
			//			{
			//				MemberName = "Pham Tan",
			//				CostsList = new List<Cost>
			//				{
			//					new Cost
			//					{
			//						PaymentName = "Pho B5",
			//						Charge = 22000
			//					},
			//					new Cost
			//					{
			//						PaymentName = "Sua chua Long Thanh",
			//						Charge = 6000
			//					}
			//				}
			//			}
			//		}
			//	}
			//};

			this.DataContext = this;
			
			//Tạo dữ liệu màu cho ListColor
			ListColor = new List<ColorSetting>
			{
				new ColorSetting { Color = "#FFCA5010"}, new ColorSetting { Color = "#FFFF8C00"}, new ColorSetting { Color = "#FFE81123"}, new ColorSetting { Color = "#FFD13438"}, new ColorSetting { Color = "#FFFF4081"},
				new ColorSetting { Color = "#FFC30052"}, new ColorSetting { Color = "#FFBF0077"}, new ColorSetting { Color = "#FF9A0089"}, new ColorSetting { Color = "#FF881798"}, new ColorSetting { Color = "#FF744DA9"},
				new ColorSetting { Color = "#FF4CAF50"}, new ColorSetting { Color = "#FF10893E"}, new ColorSetting { Color = "#FF018574"}, new ColorSetting { Color = "#FF03A9F4"}, new ColorSetting { Color = "#FF304FFE"},
				new ColorSetting { Color = "#FF0063B1"}, new ColorSetting { Color = "#FF6B69D6"}, new ColorSetting { Color = "#FF8E8CD8"}, new ColorSetting { Color = "#FF8764B8"}, new ColorSetting { Color = "#FF038387"},
				new ColorSetting { Color = "#FF525E54"}, new ColorSetting { Color = "#FF7E735F"}, new ColorSetting { Color = "#FF9E9E9E"}, new ColorSetting { Color = "#FF515C6B"}, new ColorSetting { Color = "#FF000000"}
			};
			ColorScheme = ListColor[11].Color;

			//Danh sách các giai đoạn của chuyến đi
			StageList = new List<string>
			{
				"Bắt đầu", "Đang đi", "Đến nơi", "Đang về", "Kết thúc"
			};

			//Mặc định khi mở ứng dụng thị hiển thị menu ở dạng mở rộng
			isMinimizeMenu = false;

			//Lấy danh sách food
			var trips = TripOnScreen.Take(TripPerPage);
			TripButtonItemsControl.ItemsSource = trips;
			view = (CollectionView)CollectionViewSource.GetDefaultView(TripInfoList);
			TripListAppearAnimation();

			SaveListFood();
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		//---------------------------------------- Các hàm xử lý cập nhật giao diện --------------------------------------------//

		//Cập nhật trạng thái của nút phân trang trong các trường hợp
		private void UpdatePageButtonStatus()
		{
			//Vô hiệu hóa nút quay về trang trước và quay về trang đầu khi trang hiện tại là trang 1
			if (CurrentPage == 1)
			{
				PreviousPageButton.IsEnabled = false;
				PreviousPageTextBlock.Foreground = Brushes.Black;

				FirstPageButton.IsEnabled = false;
				FirstPageTextBlock.Foreground = Brushes.Black;
			}
			else if (PreviousPageButton.IsEnabled == false)
			{
				PreviousPageButton.IsEnabled = true;
				PreviousPageTextBlock.Foreground = Brushes.White;

				FirstPageButton.IsEnabled = true;
				FirstPageTextBlock.Foreground = Brushes.White;
			}

			//Vô hiệu hóa nút đi tới trang sau và đi tới trang cuối khi trang hiện tại là trang cuối
			if (CurrentPage == TotalPage)
			{
				NextPageButton.IsEnabled = false;
				NextPageTextBlock.Foreground = Brushes.Black;

				LastPageButton.IsEnabled = false;
				LastPageTextBlock.Foreground = Brushes.Black;
			}
			else if (NextPageButton.IsEnabled == false)
			{
				NextPageButton.IsEnabled = true;
				NextPageTextBlock.Foreground = Brushes.White;

				LastPageButton.IsEnabled = true;
				LastPageTextBlock.Foreground = Brushes.White;
			}
		}




		//---------------------------------------- Các hàm lưu trữ dữ liệu --------------------------------------------//

		//Lưu lại danh sách món ăn
		private void SaveListFood()
		{
			XmlSerializer xs = new XmlSerializer(typeof(List<Trip>));
			TextWriter writer = new StreamWriter(@"Data\Trip.xml");
			xs.Serialize(writer, TripInfoList);
			writer.Close();
		}



		//---------------------------------------- Các hàm xử lý khác --------------------------------------------//

		private void TripListAppearAnimation()
		{
			ThicknessAnimation animation = new ThicknessAnimation();
			animation.AccelerationRatio = 0.9;
			animation.From = new Thickness(15, 60, 0, 0);
			animation.To = new Thickness(15, 6, 0, 0);
			animation.Duration = TimeSpan.FromSeconds(0.5);
			TripListGrid.BeginAnimation(Grid.MarginProperty, animation);
		}
	}
}
