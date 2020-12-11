using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class HomeViewModel
{
	public HomeViewModel()
	{
		_colorScheme = "Green";
	}

	private string _colorScheme;
	public string ColorScheme
	{
		get
		{
			return _colorScheme;
		}
		set
		{
			_colorScheme = value;
		}
	}
}
