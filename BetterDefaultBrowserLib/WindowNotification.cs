using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib
{
    public static class WindowNotification
    {

        public delegate void WindowsNotificationUpdateHandler(object sender, WindowNotificationEventArgs e);
        public static event WindowsNotificationUpdateHandler OnUpdateWindowsNotification;

        public static void show(object sender, String message)
        {
            //Nothing is listening
            if (OnUpdateWindowsNotification == null)
                return;

            WindowNotificationEventArgs args = new WindowNotificationEventArgs(message);
            OnUpdateWindowsNotification(sender, args);
        }
        
        public class WindowNotificationEventArgs : EventArgs
        {
            public String Message { get; private set; }

            public WindowNotificationEventArgs(String message)
            {
                Message = message;
            }
        }
    }
}
