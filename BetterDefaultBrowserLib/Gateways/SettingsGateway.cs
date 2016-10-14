using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BetterDefaultBrowser.Lib.Models;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Gateways
{
    /// <summary>
    /// Gateway to access settings file.
    /// </summary>
    public class SettingsGateway : ISettingsGateway
    {
        /// <summary>
        /// Instance of SettingsGateway
        /// </summary>
        private static SettingsGateway instance;

        /// <summary>
        /// Path to the settings file.
        /// </summary>
        private static String path;

        /// <summary>
        /// List of filters
        /// </summary>
        private List<Filter> filters = new List<Filter>();

        /// <summary>
        /// Initializes static members of the <see cref="SettingsGateway" /> class.
        /// </summary>
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

        /// <summary>
        /// Prevents a default instance of the <see cref="SettingsGateway" /> class from being created.
        /// </summary>
        private SettingsGateway()
        {
        }

        /// <summary>
        /// Gets an SettingsGateway instance.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the internal default browser.
        /// </summary>
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

        /// <summary>
        /// Gets the list of saved filters.
        /// </summary>
        public List<Filter> Filters
        {
            get
            {
                return this.filters;
            }
        }

        /// <summary>
        /// Updates a filter in the save file or adds it if it doesn't exist.
        /// </summary>
        /// <param name="filter">Filter with new information</param>
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
                this.filters.Add(filter);
            }
            else
            {
                // Count is 1
                // Replace information of this filter with the new one
                thisFilter.First().ReplaceWith(this.SerializeOneFilter(filter));
            }

            root.Save(path);

            // Sort for priorities (Descending)
            this.filters.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        /// <summary>
        /// Removes a filter from the save file.
        /// </summary>
        /// <param name="filter">Filter to remove</param>
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
            this.filters.Remove(filter);
        }

        /// <summary>
        /// Loads the filter list from the settings file.
        /// </summary>
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
                var f = this.DeserializeOneFilter(node);
                if (f != null)
                {
                    this.filters.Add(f);
                }
            }

            // Sort for priorities (Descending)
            this.filters.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        /// <summary>
        /// Deserializes xml to a filter.
        /// </summary>
        /// <param name="filterElement">XML element</param>
        /// <returns>Filter object</returns>
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

        /// <summary>
        /// Serializes a filter to xml.
        /// </summary>
        /// <param name="filter">Filter object</param>
        /// <returns>XML element</returns>
        private XElement SerializeOneFilter(Filter filter)
        {
            YAXSerializer ser = new YAXSerializer(typeof(Filter));
            var serializedFilter = ser.SerializeToXDocument(filter);
            return serializedFilter.Root;
        }
    }
}
