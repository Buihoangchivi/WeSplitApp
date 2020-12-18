using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace WeSplitApp
{
    class SumChargeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var membersList = value as BindingList<Member>;
            int sum = 0;
            foreach(var member in membersList)
            {
                foreach( var cost in member.CostsList)
                {
                    sum += cost.Charge;
                }
            }
            return sum;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}