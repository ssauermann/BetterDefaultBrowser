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
        #region Fields 
        /// <summary>
        /// Instance of SettingsGateway
        /// </summary>
        private static SettingsGateway _instance;

        /// <summary>
        /// Path to the settings file.
        /// </summary>
        private static readonly String Path;

        /// <summary>
        /// List of filters
        /// </summary>
        private readonly List<Filter> _filters = new List<Filter>();
        #endregion

        #region Constrcutors
        /// <summary>
        /// Initializes static members of the <see cref="SettingsGateway" /> class.
        /// </summary>
        static SettingsGateway()
        {
            Path = HardcodedValues.DATA_FOLDER;
            Directory.CreateDirectory(Path);

            Path += @"settings.xml";
            if (!File.Exists(Path))
            {
                new XDocument(new XElement("settings", new XElement("filters"))).Save(Path);
            }

            Instance.LoadFilters();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="SettingsGateway" /> class from being created.
        /// </summary>
        private SettingsGateway()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets an SettingsGateway instance.
        /// </summary>
        public static SettingsGateway Instance => _instance ?? (_instance = new SettingsGateway());

        /// <summary>
        /// Gets or sets the internal default browser.
        /// </summary>
        public BrowserStorage DefaultBrowser
        {
            get
            {
                var root = XElement.Load(Path);
                var @default = root.Element("default");
                if (@default == null)
                {
                    return null;
                }

                return @default.Value;
            }

            set
            {
                var root = XElement.Load(Path);
                root.SetElementValue("default", value.Key);

                root.Save(Path);
            }
        }

        /// <summary>
        /// Gets the list of saved filters.
        /// </summary>
        public List<Filter> Filters => _filters;
        #endregion

        #region Methods
        /// <summary>
        /// Updates a filter in the save file or adds it if it doesn't exist.
        /// </summary>
        /// <param name="filter">Filter with new information</param>
        public void UpdateOrAddFilter(Filter filter)
        {
            var root = XElement.Load(Path);
            var filtersOuter = root.Element("filters") ?? new XElement("filters");

            var thisFilter = from f in filtersOuter.Elements()
                             where f.Attribute("id")?.Value == filter.Id
                             select f;
            var xElements = thisFilter as XElement[] ?? thisFilter.ToArray();
            var count = xElements.Length;
            if (count > 1)
            {
                // User must had edited the list manually -> so remove the duplicate? Give them a new ID? [currently first]
                for (int i = 1; i < count; i++)
                {
                    xElements.ElementAt(i).Remove();
                }

                // Remove all from internal list and add one back
                _filters.RemoveAll(x => x.Equals(filter));
                _filters.Add(filter);
            }

            if (count == 0)
            {
                // Add the filter to the list and the xml file
                filtersOuter.Add(SerializeOneFilter(filter));
                _filters.Add(filter);
            }
            else
            {
                // Count is 1
                // Replace information of this filter with the new one
                xElements.First().ReplaceWith(SerializeOneFilter(filter));
            }

            root.Save(Path);

            // Sort for priorities (Descending)
            _filters.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

        /// <summary>
        /// Removes a filter from the save file.
        /// </summary>
        /// <param name="filter">Filter to remove</param>
        public void RemoveFilter(Filter filter)
        {
            // Remove from file
            var root = XElement.Load(Path);
            var filtersOuter = root.Element("filters");

            if (filtersOuter == null)
            {
                return;
            }

            var thisFilter = from f in filtersOuter.Elements()
                             where f.Attribute("id")?.Value == filter.Id
                             select f;

            thisFilter.Remove();
            root.Save(Path);

            // Remove from internal list
            _filters.Remove(filter);
        }

        /// <summary>
        /// Loads the filter list from the settings file.
        /// </summary>
        private void LoadFilters()
        {
            // Clear list
            _filters.Clear();

            var root = XElement.Load(Path);
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
                    _filters.Add(f);
                }
                else
                {
                    // TODO: Log error
                }
            }

            // Sort for priorities (Descending)
            _filters.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }
        #endregion

        #region Private Methods
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
                // Return filter if the XML input has been deserialized successfully
                // else return null
                var f = (Filter)o;

                return f;
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
        #endregion
    }
}
