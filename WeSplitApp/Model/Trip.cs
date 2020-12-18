using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class Trip : INotifyPropertyChanged
{
	private int tripID;
	private string tripName;
	private string location;
	private string stage;
	private string primaryImagePath;        //Đường dẫn ảnh chính
	private BindingList<TripImage> imagesList = new BindingList<TripImage>();
	private BindingList<Member> membersList = new BindingList<Member>();

	public int TripID
	{
		get
		{
			return tripID;
		}
		set
		{
			tripID = value;
			OnPropertyChanged("TripID");
		}
	}
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
	public string Stage
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
	public BindingList<TripImage> ImagesList
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
	public BindingList<Member> MembersList
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
