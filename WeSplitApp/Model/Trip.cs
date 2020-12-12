using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class Trip : INotifyPropertyChanged
{
	private string tripName;
	private string location;
	private int stage;
	private string primaryImagePath;		//Đường dẫn ảnh chính
	private bool isFavorite;				//Chuyến đi yêu thích
	private List<string> imagesList;
	private List<Member> membersList;

	public string TripName
	{
		get
		{
			return tripName;
		}
		set
		{
			tripName = value;
			OnPropertyChanged("TripName");
		}
	}
	public string Location
	{
		get
		{
			return location;
		}
		set
		{
			location = value;
			OnPropertyChanged("Location");
		}
	}
	public int Stage
	{
		get
		{
			return stage;
		}
		set
		{
			stage = value;
			OnPropertyChanged("Stage");
		}
	}
	public string PrimaryImagePath
	{
		get
		{
			return primaryImagePath;
		}
		set
		{
			primaryImagePath = value;
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs("PrimaryImagePath"));
			}
		}
	}
	public bool IsFavorite
	{
		get
		{
			return isFavorite;
		}
		set
		{
			isFavorite = value;
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs("IsFavorite"));
			}
		}
	}
	public List<string> ImagesList
	{
		get
		{
			return imagesList;
		}
		set
		{
			imagesList = value;
			OnPropertyChanged("ImagesList");
		}
	}
	public List<Member> MembersList
	{
		get
		{
			return membersList;
		}
		set
		{
			membersList = value;
			OnPropertyChanged("MembersList");
		}
	}

	#region INotifyPropertyChanged Members  

	public event PropertyChangedEventHandler PropertyChanged;
	private void OnPropertyChanged(string propertyName)
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	#endregion

}
