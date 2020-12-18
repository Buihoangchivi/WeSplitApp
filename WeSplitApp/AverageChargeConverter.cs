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
    class AverageChargeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var membersList = value as BindingList<Member>;
            int sum = 0;
            int count = membersList.Count;
            foreach (var member in membersList)
            {
                foreach (var cost in member.CostsList)
                {
                    sum += cost.Charge;
                    
                }
            }
            float res = (float)sum / (float)count;
            return res;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}