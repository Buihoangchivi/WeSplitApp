using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;

public class Trip : INotifyPropertyChanged
{
	public class TripImage : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private string _ImagePath;
		public string ImagePath
		{
			get
			{
				return _ImagePath;
			}
			set
			{
				_ImagePath = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("ImagePath"));
				}
			}
		}	
	}


	private string tripName;
	private string location;
	private List<TripImage> imagesList;
	private List<Member> membersList;
	private string _status;
	private string _avatar;
	private string _tripDestination;

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
	public List<TripImage> ImagesList
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
	public string Status
	{
		get
		{
			return _status;
		}
		set
		{
			_status = value;
			OnPropertyChanged("Status");
		}
	}

	public string Avatar
	{
		get
		{
			return _avatar;
		}
		set
		{
			_avatar = value;
			OnPropertyChanged("Avatar");
		}
	}

	public string TripDestination
	{
		get
		{
			return _tripDestination;
		}
		set
		{
			_tripDestination = value;
			OnPropertyChanged("TripDestination");
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
