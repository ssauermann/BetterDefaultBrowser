using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BetterDefaultBrowser.Converter
{
    public class IsStringNullOrEmptyConverter : MarkupExtension, IValueConverter
    {
        public bool Inverted { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // XOR inverts output if set
            return Inverted ^ string.IsNullOrEmpty(value as string);
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

