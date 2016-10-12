using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Debug
{
    /// <summary>
    /// Debugging and logging Methods.
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// Get a Console Window for debugging purposes.
        /// Now Console.WriteLine() etc. can be used.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();

        //Maybe move this? Into each file? TODO
        private static readonly Lazy<TraceSource> Log = new Lazy<TraceSource>(() => DebugHelper.Create("BetterDefaultBrowser"));

        public static TraceSource Create(string sourceName)
        {
            var source = new TraceSource(sourceName);
            source.Listeners.AddRange(Trace.Listeners);
            source.Switch.Level = SourceLevels.Warning;
            return source;
        }

        public static void SetUpListener()
        {
            var path = HardcodedValues.DATA_FOLDER + "log.txt";
            Directory.CreateDirectory(HardcodedValues.DATA_FOLDER);
            //Setup listener:
            var listener = new TextWriterTraceListener(path) { TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Callstack };
            Trace.Listeners.Add(listener);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
            //
        }

        public static void WriteLine(String msg, bool noTime = false)
        {
            if (!noTime)
                msg = "[" + DateTime.Now + "] " + msg;
            Trace.WriteLine(msg);
        }

        public static void PrintRegistryDump()
        {
            WriteLine("");
            WriteLine("----------------------------Registry Dump-------------------------------", true);
            WriteLine(Dump(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet\" + HardcodedValues.APP_NAME)), true);
            WriteLine(Dump(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\" + HardcodedValues.PROG_ID)), true);
            WriteLine(Dump(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\RegisteredApplications\" + HardcodedValues.APP_NAME)), true);
            WriteLine(Dump(Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations")), true);
            WriteLine("------------------------------------------------------------------------", true);
        }

        private static String Dump(RegistryKey obj)
        {
            StringBuilder sb = new StringBuilder();
            if (obj == null)
                return string.Empty;
            else
            {
                sb.AppendFormat("[{0}]", obj.ToString());
                sb.AppendLine();
                //Values
                foreach (var val in obj.GetValueNames())
                {
                    sb.AppendFormat("{0} = {1}", (val != null && val != "") ? val : "@", obj.GetValue(val));
                    sb.AppendLine();
                }
                sb.AppendLine();

                //Subkeys
                foreach (var key in obj.GetSubKeyNames())
                {
                    sb.Append(Dump(obj.OpenSubKey(key)));
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }
}
