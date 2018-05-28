using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using ToasterWpf.Model;
using ToasterWpf.Services;

namespace ToasterWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string AUMID = "Brave";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            ActivatorHelper.RegisterActivator<NotificationActivator>();
            ActivatorHelper.RegisterComServer(typeof(NotificationActivator),
                Process.GetCurrentProcess().MainModule.FileName);

            if (e.Args.Length > 0)
            {
                // If "-Embedding" argument is appended, it will mean this application is started by COM.
                if (e.Args.Contains("-Embedding"))
                {
                    Logger.Info("Started by COM");
                }
            }

            if (e.Args.Length == 0)
            {
                Console.WriteLine("No args provided.\n");
                PrintHelp();
            }
            else if (e.Args.Length == 1)
            {
                if (e.Args[0] == "?") PrintHelp();
                //else ShowToast(e.Args[0]);
            }
            else
            {
                var toastModel = new ToastModel();
                for (int i = 0; i < e.Args.Length; i++)
                {
                    switch (e.Args[i])
                    {
                        case "-t":
                            if (i + 1 < e.Args.Length)
                            {
                                toastModel.Title = e.Args[i + 1];
                            }
                            else
                            {
                                Console.WriteLine(
                                    "Missing argument to -t.\n Supply argument as -t \"bold title string\"\n");
                                Environment.Exit(-1);
                            }

                            break;

                        case "-b":
                            if (i + 1 < e.Args.Length)
                            {
                                toastModel.Body = e.Args[i + 1];
                            }


                            break;
                        case "-p":
                            if (i + 1 < e.Args.Length)
                            {
                                toastModel.ImagePath = e.Args[i + 1];
                            }
                            else
                            {
                                Console.WriteLine("Missing argument to -p.\n Supply argument as -p \"image path\"\n");
                                Environment.Exit(-1);
                            }

                            break;
                        case "-silent":
                            toastModel.Silent = true;
                            break;
                        //case "-w":
                        //    //wait = true;
                        //    break;
                        default: break;
                    }
                }



                var tstService = new SendToastService();
                tstService.ShowInteractiveToast(toastModel, AUMID);



                // If launched from a toast
                // This launch arg was specified in our WiX installer where we register the LocalServer32 exe path.
                //if (e.e.Args.Contains(DesktopNotificationManagerCompat.TOAST_ACTIVATED_LAUNCH_ARG))
                //{
                //    // Our NotificationActivator code will run after this completes,
                //    // and will show a window if necessary.
                //}
                //else
                //{
                //    // Show the window
                //    // In App.xaml, be sure to remove the StartupUri so that a window doesn't
                //    // get created by default, since we're creating windows ourselves (and sometimes we
                //    // don't want to create a window if handling a background activation).
                //    //new MainWindow().Show();
                //}

                // base.OnStartup(e);
                Application.Current.Shutdown();
            }

        }
        private static void PrintHelp()
        {
            String inst = "---- Usage ----\n" +
                          "toast <string>|[-t <string>][-b <string>][-p <string>]\n\n" +
                          "---- Args ----\n" +
                          "<string>\t\t| Toast <string>, no add. args will be read.\n" +
                          "[-t] <title string>\t| Displayed on the first line of the toast.\n" +
                          "[-b] <body string>\t| Displayed on the remaining lines, wrapped.\n" +
                          "[-p] <image URI>\t| Display toast with an image\n" +
                          "[-silent] \t\t\t| Deactivate sound (quiet).\n" +
                          "[-w] \t\t\t| Wait for toast to expire or activate.\n" +
                          "?\t\t\t| Print these instructions. Same as no args.\n" +
                          "Exit Status\t:  Exit Code\n" +
                          "Failed\t\t: -1\nSuccess\t\t:  0\nHidden\t\t:  1\nDismissed\t:  2\nTimeout\t\t:  3\n\n" +
                          "---- Image Notes ----\n" +
                          "Images must be .png with:\n" +
                          "\tmaximum dimensions of 1024x1024\n" +
                          "\tsize <= 200kb\n" +
                          "Windows file paths only.\n";
            Console.WriteLine(inst);
        }
    }
}
