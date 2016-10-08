using BetterDefaultBrowser.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterDefaultBrowser
{
    public class MainWindowBind : INotifyPropertyChanged
    {
        private Browser browser;
        public Browser Browser
        {
            get { return browser; }
            set
            {
                browser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Browser"));
            }
        }

        public enum Protocols { https, http, any };

        private Protocols protocol;

        public Protocols Protocol
        {
            get { return protocol; }
            set
            {
                protocol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Protocol"));
            }
        }

        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Url"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public MainWindowBind(Browser browser, Protocols protocol)
        {
            Browser = browser;
            Protocol = protocol;
        }

        public MainWindowBind()
        {
            Protocol = Protocols.any;
        }

        public void saveCurrent()
        {
            if (browser != null)
            {
                StringBuilder str = new StringBuilder();
                str.Append(ProtocolRegex());
                str.Append(@"(w{3}\.)?");
                str.Append(Regex.Escape(url));
                Filter filter = new Filter(str.ToString(), browser);
                LinkedList<Filter> ls = Settings.Filter;
                ls.AddFirst(filter);
                Settings.Filter = ls;
            }
            else
            {
                throw new Exception("user did not mark a browser");
            }
        }

        private string ProtocolRegex()
        {
            switch (protocol)
            {
                case Protocols.any:
                    return @"((h|H)(t|T){2}(p|P)(s|S)?\://)?";
                case Protocols.http:
                    return @"(h|H)(t|T){2}(p|P)\://";
                case Protocols.https:
                    return @"(h|H)(t|T){2}(p|P)(s|S)\://";
            }
            return "";
        }
    }
}
