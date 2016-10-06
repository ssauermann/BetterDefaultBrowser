using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowserLib
{
    public class Launcher
    {
        public static void Launch(String path, String param)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = path;
            proc.StartInfo.Arguments = param;
            proc.Start();
        }
    }
}
