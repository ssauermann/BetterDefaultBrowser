using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace BetterDefaultBrowser.Lib
{
    public class Settings
    {
        private String path;

        public Settings()
        {
            this.path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BetterDefaultBrowser";
            Directory.CreateDirectory(path);

            this.path += @"\settings.xml";
            if (!File.Exists(path))
            {
                new XDocument(new XElement("settings")).Save(path);
            }
        }

        public Browser OriginalDefaultBrowser
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
        public Browser DefaultBrowser
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
        
        public LinkedList<Filter> Filter
        {
            get
            {
                var root = XElement.Load(path);
                var filtersOuter = root.Element("filters");
                if (filtersOuter == null)
                    return new LinkedList<Filter>();

                var filters = filtersOuter.Elements();

                LinkedList<Filter> list = new LinkedList<Filter>();
                foreach(var filter in filters)
                {
                    list.AddLast(new Filter(Regex.Unescape(filter.Element("regex").Value), filter.Element("browser").Value));
                }

                return list;
            }
            set
            {
                var root = XElement.Load(path);
                root.SetElementValue("filters", "");
                var filtersOuter = root.Element("filters");
                

                foreach(var filter in value)
                {
                    var fNode = new XElement("filter",
                        new XElement("regex", Regex.Escape(filter.RegEx)),
                        new XElement("browser", filter.AssignedBrowser)
                        );
                    filtersOuter.Add(fNode);
                }


                root.Save(path);
            }
        }

    }
}
