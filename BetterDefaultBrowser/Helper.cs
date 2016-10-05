using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser
{
    class Helper
    {
        private static void start(String path, String arg) {
            Process proc = new Process();
            proc.StartInfo.FileName = path;
            proc.StartInfo.Arguments = arg;
            proc.Start();
            proc.WaitForExit();
            //System.Threading.Thread.Sleep(1000);
        }

        public static void startHelper(String mode)
        {
            start("BetterDefaultBrowser-Helper.exe", "-" + mode);
        }
    }
}
