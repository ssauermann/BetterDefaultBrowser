using System.Collections.Generic;
using System.ComponentModel;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.Interface
{
    public class StringWrapper : IFilterTreeElement
    {
        public StringWrapper(string name, string val)
        {
            Name = name;
            Value = val;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class BrowserWrapper : IFilterTreeElement
    {
        public BrowserWrapper(LinkedList<Browser> browsers)
        {
            foreach (var browser in browsers)
            {
                Browsers.Add(browser);
            }
        }

        public BindingList<Browser> Browsers { get; } = new BindingList<Browser>();
    }
}
