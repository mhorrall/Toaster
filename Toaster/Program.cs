using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using DesktopToast.Helper;
using DesktopToast.Wpf;
using Microsoft.QueryStringDotNET;
using Microsoft.Win32;
using NLog;
using NotificationsExtensions;
using NotificationsExtensions.Toasts;


namespace Toaster
{
    class Program
    {
        private const string AUMID = "Brave.Toaster";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            ActivatorHelper.RegisterActivator<NotificationActivator>();
            ActivatorHelper.RegisterComServer(typeof(NotificationActivator),
                Process.GetCurrentProcess().MainModule.FileName);

            if (args.Length > 0)
            {
                // If "-Embedding" argument is appended, it will mean this application is started by COM.
                if (args.Contains("-Embedding"))
                {
                    Logger.Info("Started by COM");
                }
            }

            if (args.Length == 0)
            {
                Console.WriteLine("No args provided.\n");
                PrintHelp();
            }
            else if (args.Length == 1)
            {
                if (args[0] == "?") PrintHelp();
                //else ShowToast(args[0]);
            }
            else
            {
                var toastModel = new ToastModel();
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-t":
                            if (i + 1 < args.Length)
                            {
                                toastModel.Title = args[i + 1];
                            }
                            else
                            {
                                Console.WriteLine(
                                    "Missing argument to -t.\n Supply argument as -t \"bold title string\"\n");
                                Environment.Exit(-1);
                            }

                            break;

                        case "-b":
                            if (i + 1 < args.Length)
                            {
                                toastModel.Body = args[i + 1];
                            }


                            break;
                        case "-p":
                            if (i + 1 < args.Length)
                            {
                                toastModel.ImagePath = args[i + 1];
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

        private static void SetSilent(bool useSound, XmlDocument toastXml)
        {
            var audio = toastXml.GetElementsByTagName("audio").FirstOrDefault();

            if (audio == null)
            {
                audio = toastXml.CreateElement("audio");
                var toastNode = ((XmlElement)toastXml.SelectSingleNode("/toast"));

                if (toastNode != null)
                {
                    toastNode.AppendChild(audio);
                }
            }

            var attribute = toastXml.CreateAttribute("silent");
            attribute.Value = (!useSound).ToString().ToLower();
            audio.Attributes.SetNamedItem(attribute);
        }

        private static void ToastActivated(ToastNotification sender, object e)
        {
            Console.WriteLine("Activated");
            Environment.Exit(0);
        }

        private static void ToastDismissed(ToastNotification sender, ToastDismissedEventArgs e)
        {
            String outputText = "";
            int exitCode = -1;
            switch (e.Reason)
            {
                case ToastDismissalReason.ApplicationHidden:
                    outputText = "Hidden";
                    exitCode = 1;
                    break;
                case ToastDismissalReason.UserCanceled:
                    outputText = "Dismissed";
                    exitCode = 2;
                    break;
                case ToastDismissalReason.TimedOut:
                    outputText = "Timeout";
                    exitCode = 3;
                    break;
            }
            Console.WriteLine(outputText);
            Environment.Exit(exitCode);
        }

        private static void ToastFailed(ToastNotification sender, ToastFailedEventArgs e)
        {
            Console.WriteLine("Error.");
            Environment.Exit(-1);
        }


    }
}
