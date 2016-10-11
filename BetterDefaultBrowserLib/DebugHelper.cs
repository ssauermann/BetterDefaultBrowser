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
            var listener = new TextWriterTraceListener(path);
            Trace.Listeners.Add(listener);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
            //
        }
    }
}
