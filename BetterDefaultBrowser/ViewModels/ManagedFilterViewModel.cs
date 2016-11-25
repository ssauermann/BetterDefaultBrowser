using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.ViewModels
{
    public class ManagedFilterViewModel : PlainFilterViewModelT<ManagedFilter>
    {

        #region Constructor
        public ManagedFilterViewModel(ManagedFilter filter, ISettingsGateway settingsGateway, IBrowserGateway browserGateway) : base(filter, settingsGateway, browserGateway)
        {
        }
        #endregion

    }
}
