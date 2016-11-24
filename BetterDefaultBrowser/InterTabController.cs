using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dragablz;

namespace BetterDefaultBrowser
{
    class InterTabController : IInterTabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            var childWindow = new TabWindow();

            return new NewTabHost<Window>(childWindow, childWindow.ChildWindowTabablzControl);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            return window is MainWindow ? TabEmptiedResponse.DoNothing : TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
