using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterDefaultBrowser.Lib.Models;
using BetterDefaultBrowser.Lib.Models.Enums;

namespace BetterDefaultBrowser.Interface
{
    public class FilterTreeViewModel
    {
        public FilterTreeViewModel()
        {

            var x =
                new PlainFilterTree(new PlainFilter
                {
                    Name = "My plain filter",
                    Regex = "I'm Regex",
                    //Browser = new Browser("My fake browser")
                });

            var y = new ManagedFilterTree(new ManagedFilter()
            {
                //Browser = new Browser("ABC"),
                Url = "www.foo.bar.com",
                Name = "My managed filter",
                Flags = Ignore.Parameter | Ignore.Page
            });
            var of = new OpenFilter()
            {
                Name = "My open filter",
                OnlyOpen = false,
                InnerFilter = new PlainFilter()
                {
                    Name = "My inner plain filter",
                    Regex = "I'm Legend",
                    //Browser = new Browser("My new browser")
                }
            };
            //of.Browsers.AddLast(new Browser("A Browser"));
            //of.Browsers.AddLast(new Browser("B Browser"));

            var z = new OpenFilterTree(of);


            //var y = new OpenFilter { Name = "My open filter", InnerFilter = x };


            FilterList.Add(x);
            FilterList.Add(y);
            FilterList.Add(z);
        }

        public BindingList<FilterTree> FilterList { get; } = new BindingList<FilterTree>();
    }
}