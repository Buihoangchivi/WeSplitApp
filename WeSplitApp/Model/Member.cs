using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class Member : INotifyPropertyChanged
{
	private string memberName;
	private List<Cost> costsList;

	public string MemberName
	{
		get
		{
			return memberName;
		}
		set
		{
			memberName = value;
			OnPropertyChanged("MemberName");
		}
	}
	public List<Cost> CostsList
	{
		get
		{
			return costsList;
		}
		set
		{
			costsList = value;
			OnPropertyChanged("CostsList");
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
