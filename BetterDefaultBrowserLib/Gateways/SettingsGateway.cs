using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterDefaultBrowser.Lib.Models;
using System.IO;
using System.Xml.Linq;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Gateways
{
    class SettingsGateway : ISettingsGateway
    {
        private List<Filter> filters = new List<Filter>();
        private static SettingsGateway instance;
        private static String path;

        private SettingsGateway()
        {
        }

        static SettingsGateway()
        {
            SettingsGateway.path = HardcodedValues.DATA_FOLDER;
            Directory.CreateDirectory(path);

            SettingsGateway.path += @"settings.xml";
            if (!File.Exists(SettingsGateway.path))
            {
                new XDocument(new XElement("settings", new XElement("filters"))).Save(SettingsGateway.path);
            }

            SettingsGateway.Instance.LoadFilters();
        }

        public static SettingsGateway Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingsGateway();
                }
                return instance;
            }
        }

        public Browser DefaultBrowser
        {
            get
            {
                var root = XElement.Load(path);
                var @default = root.Element("default");
                if (@default == null)
                {
                    return null;
                }

                return BrowserGateway.Instance.GetBrowser(@default.Value);
            }
            set
            {
                var root = XElement.Load(path);
                root.SetElementValue("default", value.Key);

                root.Save(path);
            }
        }

        public List<Filter> Filters
        {
            get
            {
                return filters;
            }
        }

        public void UpdateOrAddFilter(Filter filter)
        {
            var root = XElement.Load(path);
            var filtersOuter = root.Element("filters");

            var thisFilter = from f in filtersOuter.Elements()
                             where f.Attribute("id").Value == filter.ID
                             select f;
            var count = thisFilter.Count();
            if (count > 1)
            {
                // User must had edited the list manually -> so remove the duplicate? Give them a new ID? [currently first]
                for (int i = 1; i < count; i++)
                {
                    thisFilter.ElementAt(i).Remove();
                }

                // Remove all from internal list and add one back
                this.filters.RemoveAll(x => x.Equals(filter));
                this.filters.Add(filter);
            }

            if (count == 0)
            {
                // Add the filter to the list and the xml file
                filtersOuter.Add(this.SerializeOneFilter(filter));
                filters.Add(filter);
            }
            else //Count is 1
            {
                // Replace information of this filter with the new one
                thisFilter.First().ReplaceWith(this.SerializeOneFilter(filter));
            }

            root.Save(path);

            // Sort for priorities (Descending)
            filters.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        public void RemoveFilter(Filter filter)
        {
            // Remove from file
            var root = XElement.Load(path);
            var filtersOuter = root.Element("filters");

            var thisFilter = from f in filtersOuter.Elements()
                             where f.Attribute("id").Value == filter.ID
                             select f;

            thisFilter.Remove();
            root.Save(path);

            // Remove from internal list
            filters.Remove(filter);
        }

        private void LoadFilters()
        {
            // Clear list
            this.filters.Clear();

            var root = XElement.Load(path);
            var filterEle = root.Element("filters");
            if (filterEle == null)
            {
                // Nothing to load
                return;
            }

            // Deserialize each child and add if valid
            foreach (var node in filterEle.Elements())
            {
                var f = DeserializeOneFilter(node);
                if (f != null)
                {
                    filters.Add(f);
                }
            }

            // Sort for priorities (Descending)
            filters.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        private Filter DeserializeOneFilter(XElement filterElement)
        {
            YAXSerializer ser = new YAXSerializer(typeof(Filter));
            try
            {
                object o = ser.Deserialize(filterElement);
                if (o != null)
                {
                    // means that the XML input has been deserialized successfully
                    return (Filter)o;
                }
                else
                {
                    // the XML input cannot be deserialized successfully
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private XElement SerializeOneFilter(Filter filter)
        {
            YAXSerializer ser = new YAXSerializer(typeof(Filter));
            var serializedFilter = ser.SerializeToXDocument(filter);
            return serializedFilter.Root;
        }
    }
}
