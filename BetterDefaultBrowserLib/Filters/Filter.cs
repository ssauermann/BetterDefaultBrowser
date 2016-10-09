using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BetterDefaultBrowser.Lib.Filters
{
    /// <summary>
    /// Filter for request handling. If the filter is matched, an assigned browser will be used to open the url.
    /// </summary>
    public abstract class Filter : INotifyPropertyChanged
    {
        private String name;

        public event PropertyChangedEventHandler PropertyChanged;

        public Filter()
        {
            //Generate a new id, can be overwritten when loading the xml data.
            ID = newID();
        }


        #region Properties

        /// <summary>
        /// Filter type
        /// </summary>
        public FType Type { get; protected set; }

        /// <summary>
        /// ID for reference in saved file
        /// </summary>
        internal String ID { get; set; }

        /// <summary>
        /// Filter name for listing
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Match this filter to an url.
        /// </summary>
        /// <param name="url">URL to match</param>
        /// <param name="matchingResult">Matching browser or null</param>
        /// <returns>Does the filter match the url?</returns>
        public abstract bool Match(string url, out Browser matchingResult);

        /// <summary>
        /// Convert this filter to an XML representation.
        /// </summary>
        /// <returns>XML Element</returns>
        internal virtual XElement ToXML()
        {
            var e = new XElement("filter",
                       new XAttribute("type", Type),
                       new XAttribute("id", ID),
                       new XAttribute("name", Name)
                       );
            return e;
        }

        /// <summary>
        /// Loads values for this filter from an XML representation.
        /// <param name="e">XML element</param>
        /// </summary>
        internal virtual void FromXML(XElement e)
        {
            foreach (var attr in e.Attributes())
            {
                switch (attr.Name.LocalName)
                {
                    case "type":
                        try
                        {
                            if (Type != (FType)Enum.Parse(typeof(FType), attr.Value))
                                throw new FilterInvalidException("XML parsing error. Invalid type.");
                        }
                        catch (Exception ex) when (ex is ArgumentException | ex is ArgumentNullException)
                        {
                            throw new FilterInvalidException("XML parsing error. Not a type.", ex);
                        }
                        break;
                    case "id":
                        ID = attr.Value;
                        break;
                    case "name":
                        Name = attr.Value;
                        break;
                    default:
                        //Ignore so child classes can parse their attributes.
                        break;
                }
            }
        }

        /// <summary>
        /// Stores the filter into the save file.
        /// Has to be called after an filters was edited or added.
        /// </summary>
        public void Store()
        {
            Settings.saveFilter(this);
        }


        private static String newID()
        {
            return Guid.NewGuid().ToString("N");
        }

        #endregion

        #region Inner Classes
        /// <summary>
        /// Filter has invalid settings.
        /// </summary>
        public class FilterInvalidException : Exception
        {
            public FilterInvalidException()
            {

            }

            public FilterInvalidException(String msg)
                : base(msg)
            {

            }

            public FilterInvalidException(String msg, Exception cause) : base(msg, cause)
            {

            }
        }

        /// <summary>
        /// Filter types
        /// </summary>
        public enum FType
        {
            /// <summary>
            /// Just plain regex
            /// </summary>
            PLAIN,
            /// <summary>
            /// Uses first open browser
            /// </summary>
            OPEN,
            /// <summary>
            /// Fancy GUI for regex creation
            /// </summary>
            MANAGED
        }
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
