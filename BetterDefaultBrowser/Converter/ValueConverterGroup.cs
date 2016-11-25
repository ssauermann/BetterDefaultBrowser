using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace BetterDefaultBrowser.Converter
{
    /// <summary>
    /// http://stackoverflow.com/a/8326207
    /// </summary>
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
