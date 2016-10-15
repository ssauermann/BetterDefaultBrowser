using System.ComponentModel;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.Interface
{
    public abstract class FilterTree : NotifyPropertyChangedBase
    {
        protected Filter Filter;

        protected FilterTree(Filter filter, string type)
        {
            this.Filter = filter;
            this.Type = type;
        }

        public string Name => Filter.Name;

        public string Type { get; private set; }

        public BindingList<IFilterTreeElement> Attributes { get; } = new BindingList<IFilterTreeElement>();
    }

    public class PlainFilterTree : FilterTree
    {
        public PlainFilterTree(PlainFilter filter) : base(filter, "plain")
        {
            Attributes.Add(new StringWrapper("Regex", filter.Regex));
        }
    }

    public class ManagedFilterTree : FilterTree
    {
        public ManagedFilterTree(ManagedFilter filter) : base(filter, "managed")
        {
            Attributes.Add(new StringWrapper("URL", filter.URL));
            Attributes.Add(new StringWrapper("Protocols", filter.Protocols.ToString()));
            Attributes.Add(new StringWrapper("Ignore", filter.Flags.ToString()));
        }
    }

    public class OpenFilterTree : FilterTree
    {
        public OpenFilterTree(OpenFilter filter) : base(filter, "open")
        {
            Attributes.Add(new StringWrapper("Only running?", filter.OnlyOpen.ToString()));
            Attributes.Add(new BrowserWrapper(filter.Browsers));

            // Add attributes from inner filter:
            var inner = filter.InnerFilter;
            BindingList<IFilterTreeElement> innerAttr = new BindingList<IFilterTreeElement>();

            if (inner.GetType() == typeof(PlainFilter))
            {
                innerAttr = new PlainFilterTree((PlainFilter)inner).Attributes;
            }
            else if (inner.GetType() == typeof(ManagedFilter))
            {
                innerAttr = new ManagedFilterTree((ManagedFilter)inner).Attributes;
            }

            foreach (var attr in innerAttr)
            {
                Attributes.Add(attr);
            }
        }
    }
}
