using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser
{
    public class MainWindowBind:INotifyPropertyChanged
    {
        private int browser;
        public int Browser {
            get { return browser; }
            set { browser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Browser"));
            }
        }

        public enum Protocols { https, http, ftp};

        private Protocols protocol;

        public Protocols Protocol {
            get { return protocol; }
            set { protocol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Protocol"));
            }
        }

        private string url;
        public string Url {
            get { return url; }
            set { url = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Url"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public MainWindowBind(int browser,Protocols protocol)
        {
            Browser = browser;
            Protocol= protocol;
        }

        public MainWindowBind()
        {
            Browser = 1;
            Protocol = Protocols.ftp;
        }
    }
}
