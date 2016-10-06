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

        private string protocol;

        public string Protocol {
            get { return protocol; }
            set { protocol = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Protocol"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public MainWindowBind(int browser,string protocol)
        {
            Browser = browser;
            Protocol= protocol;
        }
    }
}
