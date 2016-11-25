using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using BetterDefaultBrowser.Lib.Models.Enums;

namespace BetterDefaultBrowser.Converter
{
    /// <summary>
    /// Code based on converter from PaulJ
    /// http://stackoverflow.com/a/375288/2011949
    /// </summary>
    public class FlagsEnumToBoolConverter : MarkupExtension, IValueConverter
    {
        private int _targetValue;
        public Ignore EnumValueToBind { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mask = (int)EnumValueToBind;
            _targetValue = (int)value;

            return (mask & _targetValue) != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _targetValue ^= (int)EnumValueToBind;
            return Enum.Parse(targetType, _targetValue.ToString());
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

