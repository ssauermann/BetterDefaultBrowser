using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BetterDefaultBrowserLib;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Proxy
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [STAThread]
        static void Main()
        {

            //AllocConsole();
            //Console.WriteLine("Test");
            string[] args = Environment.GetCommandLineArgs();

            var url = "";
            if (args.Length > 1)
                url = args[1];


            //Console.WriteLine(url);

            Settings settings = new Settings();

            var filters = settings.Filter;
            //Console.WriteLine(filters);
            var defBrowser = settings.DefaultBrowser;

            var selBrowser = defBrowser;

            foreach(var filter in filters)
            {
                if (filter.match(url))
                {
                    selBrowser = filter.AssignedBrowser;
                    break;
                }
            }

            //Loop catching
            if (selBrowser.Name == "BDB Proxy")
            {
                throw new ApplicationException("No wormholes allowed.");
            }

            Process proc = new Process();
            proc.StartInfo.FileName = selBrowser.ApplicationPath;
            proc.StartInfo.Arguments = url;
            proc.Start();

            //Console.ReadKey();
        }
    }
}
