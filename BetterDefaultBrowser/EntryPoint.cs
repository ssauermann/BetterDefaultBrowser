using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using BetterDefaultBrowser.Lib;
using BetterDefaultBrowser.Lib.Helpers;

namespace BetterDefaultBrowser
{
    class EntryPoint
    {
        private static String guid;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        //http://stackoverflow.com/questions/184084/how-to-force-c-sharp-net-app-to-run-only-one-instance-in-windows
        //http://stackoverflow.com/a/502327
        //http://stackoverflow.com/questions/19147/what-is-the-correct-way-to-create-a-single-instance-application

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(String[] args)
        {
            try
            {
                Logging.SetUp("Configurator");
                DebugHelper.SetUpListener();

                //Local per session mutex.
                String mutexName = @"Local\BetterDefaultBrowser";

                //Get GUID
                var assembly = typeof(EntryPoint).Assembly;
                var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
                guid = attribute.Value;

                mutexName += guid;

                //Create mutex
                bool createdNew = true;
                using (Mutex mutex = new Mutex(true, mutexName, out createdNew))
                {
                    //If app not running, open it.
                    if (createdNew)
                    {

                        //TODO: Parameter parsing (and message to other instance) to help the user to fix or report the problem.
                        if (args != null && args.Length > 0)
                        {
                            // ...
                        }
                        else
                        {

                        }
                        run();
                    }
                    else
                    {
                        //Else: Close this one and put other into foreground
                        Process current = Process.GetCurrentProcess();
                        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                        {
                            if (process.Id != current.Id)
                            {
                                //Windows into foreground (Currently MainWindow1) (TODO?)
                                SetForegroundWindow(process.MainWindowHandle);
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                DebugHelper.AllocConsole();
                Console.WriteLine("An error has occured!\n" +
                    "The error has been logged to: " + HardcodedValues.DATA_FOLDER);
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();
                Environment.Exit(1);
            }
            finally
            {
                Trace.Close();
            }
        }

        private static void run()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

    }
}
