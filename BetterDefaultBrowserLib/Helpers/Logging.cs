using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Configuration;

namespace BetterDefaultBrowser.Lib.Helpers
{
    public static class Logging
    {
        public static void SetUp(string program)
        {
            var conf = new LoggerConfiguration()
                .WriteTo.File(HardcodedValues.DATA_FOLDER + "log.txt",
                shared: true,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [" + program + "] {Message}{NewLine}{Exception}"
                )
                .MinimumLevel.Information();

            // Sets debug level if in debug configuration, else will use information as set above
            SetDebugLevel(ref conf);


            Log.Logger = conf.CreateLogger();
        }

        [Conditional("DEBUG")]
        private static void SetDebugLevel(ref LoggerConfiguration conf)
        {
            conf.MinimumLevel.Debug();
        }
    }
}
