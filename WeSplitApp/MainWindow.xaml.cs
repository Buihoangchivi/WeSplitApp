using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;

namespace WeSplitApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		//---------------------------------------- Khai báo các biến toàn cục --------------------------------------------//

		public event PropertyChangedEventHandler PropertyChanged;
		private Button clickedControlButton, clickedTypeButton;
		private List<Trip> TripInfoList = new List<Trip>();     //Danh sách thông tin tất cả các chuyến đi
		private CollectionView view;
		//private BindingList<Trip> TripOnScreen;						//Danh sách chuyến đi để hiện trên màn hình
		private BindingList<ColorSetting> ListColor;
		private Condition FilterCondition = new Condition { Type = "" };
		public Trip trip = new Trip();
		private bool isMinimizeMenu, isEditMode, IsDetailTrip;
		int selectedTripIndex = 0;

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

		//Class điều kiện để filter
		class Condition
		{
			public string Type;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// Đọc dữ liệu các món ăn từ data

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


			this.DataContext = this;

			//Tạo dữ liệu màu cho ListColor
			ListColor = new BindingList<ColorSetting>
			{
				new ColorSetting { Color = "#FFCA5010"}, new ColorSetting { Color = "#FFFF8C00"}, new ColorSetting { Color = "#FFE81123"}, new ColorSetting { Color = "#FFD13438"}, new ColorSetting { Color = "#FFFF4081"},
				new ColorSetting { Color = "#FFC30052"}, new ColorSetting { Color = "#FFBF0077"}, new ColorSetting { Color = "#FF9A0089"}, new ColorSetting { Color = "#FF881798"}, new ColorSetting { Color = "#FF744DA9"},
				new ColorSetting { Color = "#FF4CAF50"}, new ColorSetting { Color = "#FF10893E"}, new ColorSetting { Color = "#FF018574"}, new ColorSetting { Color = "#FF03A9F4"}, new ColorSetting { Color = "#FF304FFE"},
				new ColorSetting { Color = "#FF0063B1"}, new ColorSetting { Color = "#FF6B69D6"}, new ColorSetting { Color = "#FF8E8CD8"}, new ColorSetting { Color = "#FF8764B8"}, new ColorSetting { Color = "#FF038387"},
				new ColorSetting { Color = "#FF525E54"}, new ColorSetting { Color = "#FF7E735F"}, new ColorSetting { Color = "#FF9E9E9E"}, new ColorSetting { Color = "#FF515C6B"}, new ColorSetting { Color = "#FF000000"}
			};

			//Binding dữ liệu màu cho Setting Color Table
			SettingColorItemsControl.ItemsSource = ListColor;

			//
			ColorScheme = ConfigurationManager.AppSettings["ColorScheme"];

			//Mặc định khi mở ứng dụng thị hiển thị menu ở dạng mở rộng
			isMinimizeMenu = false;
			//Mặc định không ở màn hình chi tiết
			IsDetailTrip = false;

			//Mặc định không ở chế độ chỉnh sửa chuyến đi
			isEditMode = false;
			
			TripButtonItemsControl.ItemsSource = TripInfoList;
			view = (CollectionView)CollectionViewSource.GetDefaultView(TripInfoList);
			TripListAppearAnimation();

			//Default buttons
			clickedTypeButton = AllButton;
			clickedTypeButton.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			clickedControlButton = HomeButton;
		}

		public MainWindow()
		{
			InitializeComponent();
		}

		//---------------------------------------- Các hàm xử lý cập nhật giao diện --------------------------------------------//

		//Cập nhật lại thay đổi từ dữ liệu lên màn hình
		private void UpdateUIFromData()
		{
			view.Filter = Filter;
			TripButtonItemsControl.ItemsSource = TripInfoList;
			TripListAppearAnimation();
		}

		//---------------------------------------- Các hàm Get --------------------------------------------//

		//Get current app domain
		public static string GetAppDomain()
		{
			string absolutePath;
			absolutePath = AppDomain.CurrentDomain.BaseDirectory;
			return absolutePath;
		}

		//Lấy chỉ số phần tử của chuyến đi trong mảng
		private int GetElementIndexInArray(Button button)
		{
			var curTrip = new Trip();
			//Nếu nhấn hình ảnh món ăn ở màn hình Home
			if (button.Content.GetType().Name == "WrapPanel")
			{
				var wrapPanel = (WrapPanel)button.Content;
				curTrip = (Trip)wrapPanel.DataContext;
			}
			else //Nếu nhấn món ăn ở trong nút Search
			{
				curTrip = (Trip)button.DataContext;
			}

			var result = 0;
			for (int i = 0; i < TripInfoList.Count; i++)
			{
				if (curTrip == TripInfoList[i])
				{
					result = i;
					break;
				}
				else
				{
					//Do nothing
				}
			}
			return result;
		}

		private int GetMinID()
		{
			int result = 1;
			for (int i = 0; i < TripInfoList.Count; i++)
			{
				if (result < TripInfoList[i].TripID)
				{
					break;
				}
				else
				{
					result++;
				}
			}
			return result;
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


		//---------------------------------------- Xử lý cửa sổ --------------------------------------------//

		//Cài đặt nút đóng cửa sổ
		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			SaveListFood();
			var config = ConfigurationManager.OpenExeConfiguration(
				ConfigurationUserLevel.None);
			config.AppSettings.Settings["ColorScheme"].Value = ColorScheme;
			config.Save(ConfigurationSaveMode.Minimal);
			Application.Current.Shutdown();

		}
		//Cài đặt nút phóng to/ thu nhỏ cửa sổ
		private void MaximizeButton_Click(object sender, RoutedEventArgs e)
		{
			AdjustWindowSize();
		}

		//Cài đặt nút ẩn cửa sổ
		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			this.WindowState = WindowState.Minimized;
		}

		//Thay đổi kích thước cửa sổ
		//Nếu đang ở trạng thái phóng to thì thu nhỏ và ngược lại
		private void AdjustWindowSize()
		{
			var imgName = "";

			if (WindowState == WindowState.Maximized)
			{
				WindowState = WindowState.Normal;
				imgName = "Images/maximize.png";
			}
			else
			{
				WindowState = WindowState.Maximized;
				imgName = "Images/restoreDown.png";
			}

			//Lấy nguồn ảnh
			var img = new BitmapImage(new Uri(
						imgName,
						UriKind.Relative)
				);

			//Thiết lập ảnh chất lượng cao
			RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);

			//Thay đổi icon
			(MaxButton.Content as Image).Source = img;
		}



		//---------------------------------------- Các hàm sắp xếp --------------------------------------------//

		private bool Filter(object item)
		{
			bool result = true;
			var tripInfo = (Trip)item;
			if (FilterCondition.Type != "" && FilterCondition.Type != tripInfo.Stage)
			{
				result = false;
			}
			return result;
		}



		//---------------------------------------- Xử lý các nút bấm --------------------------------------------//

		private void ChangeClickedTypeButton_Click(object sender, RoutedEventArgs e)
		{
			clickedTypeButton.Foreground = Brushes.Gray;

			var button = (Button)sender;
			clickedTypeButton = button;
			button.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);

			//Hiển thị các món ăn thuộc loại thức ăn được chọn
			if (button == AllButton)
			{
				FilterCondition.Type = "";
			}
			else if (button == ProcessingButton)
			{
				FilterCondition.Type = "Bắt đầu";
			}
			else if (button == AccomplishedButton)
			{
				FilterCondition.Type = "Kết thúc";
			}
			else
			{
				//Do nothing
			}

			//Cập nhật lại giao diện
			UpdateUIFromData();
		}

		private void ChangeClickedControlButton_Click(object sender, RoutedEventArgs e)
		{
			//Tắt màu của nút hiện tại
			var wrapPanel = (WrapPanel)clickedControlButton.Content;
			var collection = wrapPanel.Children;
			var block = (TextBlock)collection[0];
			var text = (TextBlock)collection[2];
			block.Background = Brushes.Transparent;
			text.Foreground = Brushes.Black;

			//Đóng giao diện thanh chọn loại và tìm kiếm
			TypeBarDockPanel.Visibility = Visibility.Collapsed;
			//Đóng giao diện menu
			ControlStackPanel.Visibility = Visibility.Collapsed;
			//Đóng giao diện màn hình chi tiết chuyến đi
			DetailTripGrid.Visibility = Visibility.Collapsed;
			//Đóng giao diện màn hình trang chủ
			TripListGrid.Visibility = Visibility.Collapsed;
			//Đóng giao diện màn hình thêm chuyến đi mới
			AddTripGrid.Visibility = Visibility.Collapsed;
			//Đóng giao diện màn hình cài đặt
			SettingStackPanel.Visibility = Visibility.Collapsed;
			//Đóng giao diện thông tin developer
			AboutStackPanel.Visibility = Visibility.Collapsed;

			if (IsDetailTrip == true)
			{
				IsDetailTrip = false;
			}
			else
			{ 
				//Do nothing
			}

			//Hiển thị màu cho nút vừa được nhấn
			var button = (Button)sender;
			wrapPanel = (WrapPanel)button.Content;
			collection = wrapPanel.Children;
			block = (TextBlock)collection[0];
			text = (TextBlock)collection[2];
			block.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			text.Foreground = block.Background;

			//Cập nhật nút mới
			clickedControlButton = button;

			//Mở giao diện mới sau khi nhấn nút
			if (button == HomeButton)
			{
				TripListGrid.Visibility = Visibility.Visible;
				TypeBarDockPanel.Visibility = Visibility.Visible;
				ControlStackPanel.Visibility = Visibility.Visible;
			}
			else if (button == AddTripButton)
			{
				AddTripGrid.Visibility = Visibility.Visible;
				if (isEditMode == false)
				{
					int newID = GetMinID();
					trip = new Trip() { TripID = newID, Stage = "Bắt đầu"};
				}
				AddTripGrid.DataContext = trip;
			}
			else if (button == SettingButton)
			{
				SettingStackPanel.Visibility = Visibility.Visible;
				ControlStackPanel.Visibility = Visibility.Visible;
				var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
				bool showSplashStatus = bool.Parse(value);
				if (showSplashStatus == true)
				{
					ShowSplashScreenCheckBox.IsChecked = true;
				}
			}
			else if (button == AboutButton)
			{
				AboutStackPanel.Visibility = Visibility.Visible;
				ControlStackPanel.Visibility = Visibility.Visible;
			}

			//Cập nhật lại giao diện
			UpdateUIFromData();
		}

		private void AddChargeButton_Click(object sender, RoutedEventArgs e)
		{
			var member = ((Button)sender).DataContext as Member;
			member.CostsList.Add(new Cost());
		}

		private void AddMemeberButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedTrip = ((Button)sender).DataContext as Trip;
			selectedTrip.MembersList.Add(new Member());

		}

		private void DeleteChargeButton_Click(object sender, RoutedEventArgs e)
		{
			var member = ((Button)sender).DataContext as Member;
			if (member.CostsList.Count >= 1)
			{
				member.CostsList.Remove(member.CostsList[member.CostsList.Count - 1]);
			}
			else
			{
				MessageBox.Show($"{member.MemberName} không còn khoản chi nào để xoá!", "Warning!!", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void DeleteMemeberButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedTrip = ((Button)sender).DataContext as Trip;
			if (selectedTrip.MembersList.Count >= 1)
			{
				selectedTrip.MembersList.Remove(selectedTrip.MembersList[selectedTrip.MembersList.Count - 1]);
			}
			else
			{
				MessageBox.Show("Không còn thành viên nào để xoá!", "Warning!!", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}

		private void DeleteImageButton_Click(object sender, RoutedEventArgs e)
		{
			trip.ImagesList.Remove(ImagesListView.SelectedItem as TripImage);
		}

		private void AddImageButton_Click(object sender, RoutedEventArgs e)
		{

			var fileDialog = new OpenFileDialog();
			fileDialog.Multiselect = true;
			fileDialog.Filter = "Image Files(*.JPG*;*.JPEG*;*.PNG*)|*.JPG;*.JPEG*;*.PNG*";
			fileDialog.Title = "Select Image";

			if (fileDialog.ShowDialog() == true)
			{
				var fileNames = fileDialog.FileNames;
				foreach (var filename in fileNames)
				{
					trip.ImagesList.Add(new TripImage(filename));
				}
			}
		}

		private void SaveTripButton_Click(object sender, RoutedEventArgs e)
		{
			if (isEditMode == false)
			{
				string appFolder = GetAppDomain();
				for (int i = 0; i < trip.ImagesList.Count; i++)
				{
					var imageExtension = System.IO.Path.GetExtension(trip.ImagesList[i].ImagePath);
					var newImageName = $"Images/{trip.TripID}_{i}{imageExtension}";
					var newPath = appFolder + newImageName;
					File.Copy(trip.ImagesList[i].ImagePath, newPath, true);
					trip.ImagesList[i].ImagePath = newImageName;
				}
				trip.PrimaryImagePath = trip.ImagesList[0].ImagePath;
				TripInfoList.Add(trip);
			}
			else
			{
				string appFolder = GetAppDomain();
				for (int i = 0; i < trip.ImagesList.Count; i++)
				{
					TripImage currentImage = trip.ImagesList[i];
					var imageExtension = System.IO.Path.GetExtension(currentImage.ImagePath);
					var newImageName = $"Images/{trip.TripID}_{i}{imageExtension}";
					var newPath = appFolder + newImageName;
					if (System.IO.Path.IsPathRooted(currentImage.ImagePath))
					{
						File.Copy(currentImage.ImagePath, newPath, true);
						trip.ImagesList[i].ImagePath = newImageName;
					}
					else
					{
						if (currentImage.ImagePath != TripInfoList[selectedTripIndex].ImagesList[i].ImagePath)
						{
							File.Delete(appFolder + TripInfoList[selectedTripIndex].ImagesList[i].ImagePath);
							File.Move(appFolder + currentImage.ImagePath, newPath);
							currentImage.ImagePath = newImageName;
						}
					}
				}
				if (trip.ImagesList.Count > 0)
				{
					trip.PrimaryImagePath = trip.ImagesList[0].ImagePath;
				}
				else
				{
					trip.PrimaryImagePath = "";
				}
				TripInfoList[selectedTripIndex] = trip;
			}

			//Đóng giao diện thêm/chỉnh sửa và mở giao diện trang chủ
			CancelTripButton_Click(null, null);
		}

		private void CancelTripButton_Click(object sender, RoutedEventArgs e)
		{
			//Đóng màn hình thêm chuyến đi
			AddTripGrid.Visibility = Visibility.Collapsed;
			//Tắt màu của nút Add
			var wrapPanel = (WrapPanel)AddTripButton.Content;
			var collection = wrapPanel.Children;
			var block = (TextBlock)collection[0];
			var text = (TextBlock)collection[2];
			block.Background = Brushes.Transparent;
			text.Foreground = Brushes.Black;

			if (isEditMode == true)
			{
				//Quay ve man hinh chi tiet
				DetailTripGrid.DataContext = TripInfoList[selectedTripIndex];
				DetailTripGrid.Visibility = Visibility.Visible;
				ControlStackPanel.Visibility = Visibility.Visible;

				//Tắt chế độ chỉnh sửa
				isEditMode = false;
				IsDetailTrip = true;
			}
			else
			{
				//Quay về màn hình Home
				clickedControlButton = HomeButton;
				TripListGrid.Visibility = Visibility.Visible;
				TypeBarDockPanel.Visibility = Visibility.Visible;
				ControlStackPanel.Visibility = Visibility.Visible;
			}

			//Hiển thị màu cho nút Home
			wrapPanel = (WrapPanel)HomeButton.Content;
			collection = wrapPanel.Children;
			block = (TextBlock)collection[0];
			text = (TextBlock)collection[2];
			block.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			text.Foreground = block.Background;

			clickedControlButton = HomeButton;

			//Cập nhật lại giao diện
			UpdateUIFromData();
		}

		private void EditTripButton_Click(object sender, RoutedEventArgs e)
		{
			isEditMode = true;
			trip = new Trip(TripInfoList[selectedTripIndex]);
			//Bật màn hình chỉnh sửa
			ChangeClickedControlButton_Click(AddTripButton, null);
		}

		private void MenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (isMinimizeMenu == false)
			{
				col0.Width = new GridLength(46);
				//TripPerPage = 15;
				//UpdateFoodStatus();
				isMinimizeMenu = true;
			}
			else
			{
				col0.Width = new GridLength(250);
				//TripPerPage = 12;
				//UpdateFoodStatus();
				isMinimizeMenu = false;
			}
		}

		private void DeleteTripButton_Click(object sender, RoutedEventArgs e)
		{
			TripInfoList.Remove(TripInfoList[selectedTripIndex]);

			//
			DetailTripGrid.Visibility = Visibility.Collapsed;
			//Tắt màu của nút Add
			var wrapPanel = (WrapPanel)AddTripButton.Content;
			var collection = wrapPanel.Children;
			var block = (TextBlock)collection[0];
			var text = (TextBlock)collection[2];
			block.Background = Brushes.Transparent;
			text.Foreground = Brushes.Black;

			//Quay về màn hình Home
			clickedControlButton = HomeButton;
			TripListGrid.Visibility = Visibility.Visible;
			TypeBarDockPanel.Visibility = Visibility.Visible;
			ControlStackPanel.Visibility = Visibility.Visible;
			//Hiển thị màu cho nút Home
			wrapPanel = (WrapPanel)HomeButton.Content;
			collection = wrapPanel.Children;
			block = (TextBlock)collection[0];
			text = (TextBlock)collection[2];
			block.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			text.Foreground = block.Background;

			//Tắt chế độ chỉnh sửa
			isEditMode = false;

			//Cập nhật lại giao diện
			UpdateUIFromData();
		}

		private void Trip_Click(object sender, RoutedEventArgs e)
		{
			//Đóng giao diện màn hình danh sách các chuyến đi
			TripListGrid.Visibility = Visibility.Collapsed;
			//Đóng giao diện thanh chọn loại chuyến đi
			TypeBarDockPanel.Visibility = Visibility.Collapsed;

			//Lấy chỉ số của hình ảnh món ăn được nhấn
			selectedTripIndex = GetElementIndexInArray((Button)sender);
			DetailTripGrid.DataContext = TripInfoList[selectedTripIndex];
			trip = new Trip(TripInfoList[selectedTripIndex]);

			//Mở giao diện màn hình chi tiết chuyến đi
			DetailTripGrid.Visibility = Visibility.Visible;

			//Bật chế độ đang ở màn hình chi tiết
			IsDetailTrip = true;
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



        private void ChargePie_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ChargePie.Series = new SeriesCollection();
			((DefaultTooltip)ChargePie.DataTooltip).SelectionMode = TooltipSelectionMode.OnlySender;
			foreach (var member in trip.MembersList)
			{
				ChargePie.Series.Add(
						new PieSeries()
						{
							Values = new ChartValues<decimal> { member.CostsList.Sum(value => value.Charge) },
							Title = member.MemberName,
						}
					); ;
			}
		}

		private void ChargeChart_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			ChargeChart.Series = new SeriesCollection();
			((DefaultTooltip)ChargeChart.DataTooltip).SelectionMode = TooltipSelectionMode.OnlySender;
			ChargeChart.AxisY = new AxesCollection();
			foreach (var member in trip.MembersList)
			{
				foreach(var cost in member.CostsList)
                {
					ChargeChart.Series.Add(new ColumnSeries()
					{
						Values = new ChartValues<decimal> { cost.Charge },
						Title = cost.PaymentName
					}); ;
                }
			}

		}
		private void memberSummaryTextBlock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var index = AverageChargeTextBlock.Text.IndexOf(" ");
			if (TripInfoList.Count > 0)
			{
				if (index != -1)
				{
					double averageCharge = double.Parse(AverageChargeTextBlock.Text.Substring(0, index));
					var member = ((TextBlock)sender).DataContext as Member;
					averageCharge *= ConvertUnitStringIntoInt(AverageChargeTextBlock.Text.Substring(index + 1));
					double res = member.Deposits - averageCharge;
					((TextBlock)sender).Text = ConvertMoneyUnit(res);
					if (res < 0)
					{
						((TextBlock)sender).Foreground = Brushes.Red;
					}
					else
					{
						((TextBlock)sender).Foreground = Brushes.ForestGreen;
					}
				}
			}
		}

		private void AverageChargeTextBlock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (selectedTripIndex < TripInfoList.Count)
			{
				int sum = 0;
				if (TripInfoList.Count > 0)
				{
					int count = TripInfoList[selectedTripIndex].MembersList.Count;
					foreach (var member in TripInfoList[selectedTripIndex].MembersList)
					{
						foreach (var cost in member.CostsList)
						{
							sum += cost.Charge;

						}
					}
					double res = 0;
					if (count != 0)
					{
						res = (double)sum / count;
					}
					((TextBlock)sender).Text = ConvertMoneyUnit(res);
				}
			}
		}

		private void SumChargeTextBlock_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (selectedTripIndex < TripInfoList.Count)
			{
				int sum = 0;
				if (TripInfoList.Count > 0)
				{
					foreach (var member in TripInfoList[selectedTripIndex].MembersList)
					{
						foreach (var cost in member.CostsList)
						{
							sum += cost.Charge;

						}
					}
				((TextBlock)sender).Text = ConvertMoneyUnit(sum);
				}
			}
			else
			{
				//Do nothing
			}
		}

		private void memberSummaryTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			memberSummaryTextBlock_IsVisibleChanged(sender, new DependencyPropertyChangedEventArgs());
		}

		private void AverageChargeTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			AverageChargeTextBlock_IsVisibleChanged(sender, new DependencyPropertyChangedEventArgs());
		}

		private void SumChargeTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			SumChargeTextBlock_IsVisibleChanged(sender, new DependencyPropertyChangedEventArgs());
		}

		/*tim kiem*/
		private string ConvertToUnSign(string input)
		{
			if (input != null)
			{
				input = input.Trim();
				for (int i = 0x20; i < 0x30; i++)
				{
					input = input.Replace(((char)i).ToString(), " ");
				}
				Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
				string str = input.Normalize(NormalizationForm.FormD);
				string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
				while (str2.IndexOf("?") >= 0)
				{
					str2 = str2.Remove(str2.IndexOf("?"), 1);
				}
				return str2;
			}
			else
			{
				var res = "";
				return res;
			}
		}

		private void searchTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (e.Text != "\u001b")  //khác escapes
			{
				searchComboBox.IsDropDownOpen = true;
			}
			if (SearchFilter.IsChecked == false)
			{
				if (!string.IsNullOrEmpty(searchTextBox.Text))
				{
					string fullText = ConvertToUnSign(searchTextBox.Text.Insert(searchTextBox.CaretIndex, (e.Text)));
					searchComboBox.ItemsSource = TripInfoList.Where(s => ConvertToUnSign(s.Location).IndexOf(fullText, StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
					if (searchComboBox.Items.Count == 0)
					{
						SearchNotificationComboBox.IsDropDownOpen = true;
						searchComboBox.IsDropDownOpen = false;
					}
				}
				else if (!string.IsNullOrEmpty(e.Text))
				{
					searchComboBox.ItemsSource = TripInfoList.Where(s => ConvertToUnSign(s.Location).IndexOf(ConvertToUnSign(e.Text), StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
				}
				else
				{
					searchComboBox.ItemsSource = TripInfoList;
				}
			}
            else
            {
				if (!string.IsNullOrEmpty(searchTextBox.Text))
				{
					string fullText = ConvertToUnSign(searchTextBox.Text.Insert(searchTextBox.CaretIndex, (e.Text)));
					searchComboBox.ItemsSource = TripInfoList.Where(s => s.MembersList.Any(p => ConvertToUnSign(p.MemberName).IndexOf(fullText, StringComparison.InvariantCultureIgnoreCase) != -1)).ToList();
					if (searchComboBox.Items.Count == 0)
					{
						SearchNotificationComboBox.IsDropDownOpen = true;
						searchComboBox.IsDropDownOpen = false;
					}
				}
				else if (!string.IsNullOrEmpty(e.Text))
				{
					searchComboBox.ItemsSource = TripInfoList.Where(s => s.MembersList.Any(p => ConvertToUnSign(p.MemberName).IndexOf(ConvertToUnSign(e.Text), StringComparison.InvariantCultureIgnoreCase) != -1)).ToList();
				}
				else
				{
					searchComboBox.ItemsSource = TripInfoList;
				}
			}
		}

		private void PreviewKeyUp_EnhanceTextBoxSearch(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back || e.Key == Key.Delete)
			{


				searchComboBox.IsDropDownOpen = true;

				if (SearchFilter.IsChecked == false)
				{
					if (!string.IsNullOrEmpty(searchTextBox.Text))
					{
						searchComboBox.ItemsSource = TripInfoList.Where(s => ConvertToUnSign(s.Location).IndexOf(ConvertToUnSign(searchTextBox.Text), StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
						if (searchComboBox.Items.Count == 0)
						{
							SearchNotificationComboBox.IsDropDownOpen = true;
							searchComboBox.IsDropDownOpen = false;
						}

					}
					else
					{
						searchComboBox.ItemsSource = TripInfoList;
					}
				}
                else
                {
					if (!string.IsNullOrEmpty(searchTextBox.Text))
					{
						searchComboBox.ItemsSource = TripInfoList.Where(s => s.MembersList.Any(p => ConvertToUnSign(p.MemberName).IndexOf(ConvertToUnSign(searchTextBox.Text), StringComparison.InvariantCultureIgnoreCase) != -1)).ToList();
						if (searchComboBox.Items.Count == 0)
						{
							SearchNotificationComboBox.IsDropDownOpen = true;
							searchComboBox.IsDropDownOpen = false;
						}

					}
					else
					{
						searchComboBox.ItemsSource = TripInfoList;
					}
				}
			}
		}
		private void searchTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Down)
			{
				searchComboBox.Focus();
				searchComboBox.SelectedIndex = 0;
				searchComboBox.IsDropDownOpen = true;
			}
			if (e.Key == Key.Escape)
			{
				searchComboBox.IsDropDownOpen = false;

			}
		}

		private void Pasting_EnhanceTextSearch(object sender, DataObjectPastingEventArgs e)
		{
			searchComboBox.IsDropDownOpen = true;

			string pastedText = (string)e.DataObject.GetData(typeof(string));
			string fullText = searchTextBox.Text.Insert(searchTextBox.CaretIndex, (pastedText));

			if (!string.IsNullOrEmpty(fullText))
			{
				searchComboBox.ItemsSource = TripInfoList.Where(s => ConvertToUnSign(s.Location).IndexOf(ConvertToUnSign(fullText), StringComparison.InvariantCultureIgnoreCase) != -1).ToList();
				if (searchComboBox.Items.Count == 0)
				{
					SearchNotificationComboBox.IsDropDownOpen = true;
					searchComboBox.IsDropDownOpen = false;
				}
			}
			else
			{
				searchComboBox.ItemsSource = TripInfoList;
			}
		}

		private void searchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int index = searchComboBox.SelectedIndex;
			if (index >= 0)
			{
				var selectedTrip = searchComboBox.SelectedItem as Trip;
				string textSelected = selectedTrip.TripName;
				searchTextBox.Text = textSelected;
			}


		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			KeyUp += OnKeyUp;
		}

		void OnKeyUp(object sender, KeyEventArgs e)
		{

			if (e.Key == Key.Enter)
			{
				if (searchComboBox.SelectedIndex >= 0)
				{
					Button button = new Button();
					button.DataContext = searchComboBox.SelectedItem as Trip;
					button.Content = "button";
					searchComboBox.SelectedIndex = -1;
					SearchTripButton_Click(button, null);

				}

			}
		}

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
			e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
		}

		private void ColorButton_Click(object sender, RoutedEventArgs e)
		{
			var datatContex = (sender as Button).DataContext;
			var color = (datatContex as ColorSetting).Color;
			ColorScheme = color;
			TitleBar.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorScheme);
			SettingTextBlock.Background = TitleBar.Background;
			clickedTypeButton.Foreground = TitleBar.Background;
			SettingTitleTextBlock.Foreground = SettingTextBlock.Background;
		}

		private void ShowSplashScreenCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings["ShowSplashScreen"].Value = "true";
			config.Save(ConfigurationSaveMode.Minimal);
		}

		private void ShowSplashScreenCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings["ShowSplashScreen"].Value = "false";
			config.Save(ConfigurationSaveMode.Minimal);
		}

		private void DeleteTextInSearchButton_Click(object sender, RoutedEventArgs e)
		{
			searchTextBox.Text = "";
			searchTextBox.Focus();
		}

		private void DeleteTextInSearchButton_MouseEnter(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				DeleteTextInSearchButton_Click(null, null);
			}
			else
			{
				//Do nothing
			}
		}

		private void SearchTripButton_Click(object sender, RoutedEventArgs e)
		{
			Trip_Click(sender, null);
		}

		private string ConvertMoneyUnit(double value)
		{
			var unit = "";
			if (Math.Abs(value) < 1000)
			{
				unit = " Đồng";
			}
			else if (Math.Abs(value) < 1000000)
			{
				value /= 1000;
				unit = " Nghìn";
			}
			else if (Math.Abs(value) < 1000000000)
			{
				value /= 1000000;
				unit = " Triệu";
			}
			value = Math.Abs(Math.Round(value, 2));
			var res = value.ToString() + unit;
			return res;
		}

		private int ConvertUnitStringIntoInt(string unit)
		{
			var res = 1;
			if (unit == "Nghìn")
			{
				res = 1000;
			}
			else if (unit == "Triệu")
			{
				res = 1000000;
			}
			return res;
		}
	}
}
