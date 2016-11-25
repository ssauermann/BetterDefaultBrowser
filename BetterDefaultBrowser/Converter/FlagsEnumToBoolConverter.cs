using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BetterDefaultBrowser.Converter
{
    /// <summary>
    /// Code based on converter from PaulJ
    /// http://stackoverflow.com/a/375288/2011949
    /// </summary>
    public class FlagsEnumToBoolConverter : MarkupExtension, IValueConverter
    {
        private int _targetValue;
        public int EnumValueToBind { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mask = EnumValueToBind;
            _targetValue = (int)value;

            return (mask & _targetValue) != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _targetValue ^= EnumValueToBind;
            return Enum.Parse(targetType, _targetValue.ToString());
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

