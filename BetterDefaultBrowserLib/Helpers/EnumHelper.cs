using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Helpers
{
    /// <summary>
    /// Based on code by Nick
    /// http://stackoverflow.com/a/12430331
    /// </summary>
    public static class EnumHelper
    {
        public static string Description(this Enum eValue)
        {
            var nAttributes = eValue.GetType().GetField(eValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (nAttributes.Any())
                return (nAttributes.First() as DescriptionAttribute)?.Description;

            // If no description is found, the least we can do is replace underscores with spaces
            TextInfo oTi = CultureInfo.CurrentCulture.TextInfo;
            return oTi.ToTitleCase(oTi.ToLower(eValue.ToString().Replace("_", " ")));
        }

        public static List<Tuple<Enum, string>> GetAllValuesAndDescriptions<T>() where T : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enum type");

            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(e => new Tuple<Enum, string>(e, e.Description())).ToList();
        }
    }

}
