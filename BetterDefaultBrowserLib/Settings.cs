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

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Read and write settings to a XML file.
    /// </summary>
    public static class Settings
    {
        private static String path;

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
                new XDocument(new XElement("settings")).Save(path);
            }
        }

        /// <summary>
        /// The browser the user had set before setting BDB as the default.
        /// Can't be used to reset the user browser when uninstalling BDB since Win8.
        /// </summary>
        public static Browser OriginalDefaultBrowser
        {
            get
            {
                var root = XElement.Load(path);
                var originalDefault = root.Element("originalDefault");

                return new Browser(originalDefault.Value);
            }
            set
            {
                var root = XElement.Load(path);
                root.SetElementValue("originalDefault", value.KeyName);

                root.Save(path);
            }
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
        public static LinkedList<Filter> Filter
        {
            get
            {
                var root = XElement.Load(path);
                var filtersOuter = root.Element("filters");
                if (filtersOuter == null)
                    return new LinkedList<Filter>();

                var filters = filtersOuter.Elements();

                LinkedList<Filter> list = new LinkedList<Filter>();
                foreach (var filter in filters)
                {
                    Filter fil;
                    switch ((FType)Enum.Parse(typeof(FType),filter.Attribute("type").Value))
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

                    list.AddLast(fil);
                }

                return list;
            }
        }

        internal static void saveFilter(Filter filter)
        {
            var root = XElement.Load(path);
            root.SetElementValue("filters", "");
            var filtersOuter = root.Element("filters");

            var thisFilter = from f in filtersOuter.Elements()
                             where f.Attribute("id").Value == filter.ID
                             select f;
            var count = thisFilter.Count();
            if (count > 1)
            {
                //User must had edited the list manually -> so remove the duplicate? Give them a new ID? [currently first]
                for(int i=1;i<count;i++) {
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

        }
    }
}
