using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BetterDefaultBrowser.Converter
{

    public class BoolToStringConverter : MarkupExtension, IValueConverter
    {
        public string TrueString { get; set; } = "";
        public string FalseString { get; set; } = "";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return TrueString;
            }
            return FalseString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

