using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly Settings _settings;

        private readonly string _path;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the <see cref="SettingsGateway" /> class.
        /// <para ></para>
        /// </summary>
        public SettingsGateway(string filePath)
        {
            try
            {
                // Create directory
                {
                    var dir = Path.GetDirectoryName(filePath);
                    if (dir == null)
                    {
                        throw new ArgumentException("Path is invalid.", nameof(filePath));
                    }
                    if (dir != "")
                    {
                        Directory.CreateDirectory(dir);
                    }
                }

                // Create file
                _path = filePath;
                if (!File.Exists(_path))
                {
                    new XDocument(new XElement("settings")).Save(_path);
                }

                // Deserialize
                var ser = new YAXSerializer(typeof(Settings), YAXExceptionHandlingPolicies.ThrowErrorsOnly);
                var root = XElement.Load(_path);
                object o = ser.Deserialize(root);
                _settings = (Settings)o;
                if (_settings == null)
                {
                    throw new YAXException("Deserialization failed.");
                }

                // Create list if not deserialized:
                if (_settings.Filters == null)
                {
                    _settings.Filters = new List<Filter>();
                }

            }
            catch (Exception ex) when (ex is YAXException || ex is IOException || ex is ArgumentException)
            {
                throw new ArgumentException("Settings file can't be parsed.", nameof(filePath), ex);
            }

        }
        #endregion



        #region Properties

        /// <summary>
        /// Gets or sets the internal default browser.
        /// </summary>
        public BrowserStorage DefaultBrowser
        {
            get { return _settings.DefaultBrowser; }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (!value.Equals(_settings.DefaultBrowser))
                {
                    _settings.DefaultBrowser = value;
                    StoreSettings();
                }
            }
        }
        #endregion

        #region Methods


        private void StoreSettings()
        {
            var ser = new YAXSerializer(typeof(Settings));
            var serialized = ser.SerializeToXDocument(_settings);
            new XDocument(serialized).Save(_path);
        }

        /// <summary>
        /// Returns a shallow-copied list of all filters.
        /// </summary>
        public List<Filter> GetFilters()
        {
            return new List<Filter>(_settings.Filters);
        }

        /// <summary>
        /// Updates a filter in the save file or adds it if it doesn't exist.
        /// </summary>
        /// <param name="filter">Filter with new information</param>
        public void UpdateOrAddFilter(Filter filter)
        {
            var index = _settings.Filters.IndexOf(filter);
            if (index == -1)
            {
                // Add filter
                _settings.Filters.Add(filter);
            }
            else
            {
                // Update filter
                _settings.Filters[index] = filter;
            }

            StoreSettings();
        }

        /// <summary>
        /// Removes a filter from the save file.
        /// </summary>
        /// <param name="filter">Filter to remove</param>
        public void RemoveFilter(Filter filter)
        {
            _settings.Filters.Remove(filter);
            StoreSettings();
        }
        #endregion
    }
}
