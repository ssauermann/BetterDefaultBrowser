using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using BetterDefaultBrowser.Lib.Filters;
using static BetterDefaultBrowser.Lib.Filters.Filter;
using System.ComponentModel;

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Read and write settings to a XML file.
    /// </summary>
    public static class Settings
    {
        private static String path;
        private static BindingList<Filter> filters = new BindingList<Filter>();

        /// <summary>
        /// Create folder and settings file if not existing.
        /// </summary>
        static Settings()
        {
            path = HardcodedValues.DATA_FOLDER;
            Directory.CreateDirectory(path);

            path += @"settings.xml";
            if (!File.Exists(path))
            {
                new XDocument(new XElement("settings", new XElement("filters"))).Save(path);
            }

            loadFilters();
        }

        //TODO List
        /// <summary>
        /// The browser the requests should be send to which are not matched by a filter.
        /// </summary>
        public static Browser DefaultBrowser
        {
            get
            {
                var root = XElement.Load(path);
                var @default = root.Element("default");
                if (@default == null)
                    return null;

                return new Browser(@default.Value);
            }
            set
            {
                var root = XElement.Load(path);
                root.SetElementValue("default", value.KeyName);

                root.Save(path);
            }
        }

        /// <summary>
        /// List of filters to match for in order of priority.
        /// </summary>
        public static BindingList<Filter> Filter
        {
            get
            {
                return Settings.filters;
            }
        }

        internal static void loadFilters()
        {
            Settings.filters.Clear();

            var root = XElement.Load(path);
            var filtersOuter = root.Element("filters");
            if (filtersOuter == null)
                return;

            var filters = filtersOuter.Elements();

            foreach (var filter in filters)
            {
                Filter fil;
                switch ((FType)Enum.Parse(typeof(FType), filter.Attribute("type").Value))
                {
                    case FType.PLAIN:
                        fil = new PlainFilter();
                        break;
                    case FType.MANAGED:
                        fil = new ManagedFilter();
                        break;
                    case FType.OPEN:
                        fil = new OpenFilter();
                        break;
                    default:
                        throw new NotImplementedException("Filter type not implemented!");
                }
                fil.FromXML(filter);

                Settings.filters.Add(fil);
            }
        }

        internal static void deleteFilter(Filter filter)
        {
            var root = XElement.Load(path);
            var filtersOuter = root.Element("filters");

            var thisFilter = from f in filtersOuter.Elements()
                             where f.Attribute("id").Value == filter.ID
                             select f;

            thisFilter.Remove();
            root.Save(path);

            Settings.filters.Remove(filter);
        }

        internal static void saveFilter(Filter filter)
        {
            var root = XElement.Load(path);
            var filtersOuter = root.Element("filters");

            var thisFilter = from f in filtersOuter.Elements()
                             where f.Attribute("id").Value == filter.ID
                             select f;
            var count = thisFilter.Count();
            if (count > 1)
            {
                //User must had edited the list manually -> so remove the duplicate? Give them a new ID? [currently first]
                for (int i = 1; i < count; i++)
                {
                    thisFilter.ElementAt(i).Remove();
                }
            }

            if (count == 0)
            {
                //Add the filter to the list and the xml file
                filtersOuter.Add(filter.ToXML());
            }
            else //Count is 1
            {
                //Replace information of this filter with the new one
                thisFilter.First().ReplaceWith(filter.ToXML());
            }

            root.Save(path);
            //Add to list
            Settings.filters.Add(filter);

        }
    }
}
