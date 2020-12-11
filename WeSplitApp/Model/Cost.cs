using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class Cost : INotifyPropertyChanged
{
	private string paymentName;
	private int charge;

	public string PaymentName
	{
		get
		{
			return paymentName;
		}
		set
		{
			paymentName = value;
			OnPropertyChanged("PaymentName");
		}
	}
	public int Charge
	{
		get
		{
			return charge;
		}
		set
		{
			charge = value;
			OnPropertyChanged("Charge");
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
