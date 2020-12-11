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
