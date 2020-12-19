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
	private BindingList<TripImage> imagesList;
	private BindingList<Member> membersList;

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
	public Trip()
	{
		imagesList = new BindingList<TripImage>();
		MembersList = new BindingList<Member>();
	}

	/*Phương thức khởi tạo để chỉnh sửa chuyến đi*/
	public Trip(Trip oldTrip)
	{
		tripID = oldTrip.tripID;
		tripName = string.Copy(oldTrip.TripName);
		location = string.Copy(oldTrip.Location);
		stage = string.Copy(oldTrip.Stage);
		primaryImagePath = string.Copy(oldTrip.PrimaryImagePath);

		imagesList = new BindingList<TripImage>();

		foreach (var image in oldTrip.ImagesList)
		{
			imagesList.Add(new TripImage(image.ImagePath));
		}

		membersList = new BindingList<Member>();

		foreach (var member in oldTrip.MembersList)
		{
			if (member.MemberName != null)
			{
				membersList.Add(new Member()
				{
					MemberName = string.Copy(member.MemberName)
				});
			}
			else
			{
				membersList.Add(new Member()
				{
					MemberName = ""
				});
			}


			membersList[membersList.Count - 1].Deposits = int.Parse(member.Deposits.ToString());

			foreach (var cost in member.CostsList)
			{
				membersList[membersList.Count - 1].CostsList.Add(new Cost()
				{
					Charge = cost.Charge,
					PaymentName = cost.PaymentName
				}); ;
			}
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
