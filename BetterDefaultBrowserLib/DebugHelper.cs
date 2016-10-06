using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowserLib.Debug
{
    /// <summary>
    /// Debugging and logging Methods.
    /// </summary>
    public class DebugHelper
    {
        /// <summary>
        /// External console allocation.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        /// <summary>
        /// Get a Console Window for debugging purposes.
        /// Now Console.WriteLine() etc. can be used.
        /// </summary>
        /// <returns></returns>
        public static void Alloc()
        {
            AllocConsole();
        }

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
            //Setup listener:
            var listener = new TextWriterTraceListener(HardcodedValues.DATA_FOLDER + "log.txt");
            Trace.Listeners.Add(listener);
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
            //
        }
    }
}
