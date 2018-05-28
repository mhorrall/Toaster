﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Microsoft.QueryStringDotNET;
using NLog;

namespace Toaster
{
	/// <summary>
	/// Inherited class of notification activator (for Action Center of Windows 10)
	/// </summary>
	/// <remarks>The CLSID of this class must be unique for each application.</remarks>
	[Guid("94697601-C2EF-4097-A0EC-800B4DB37E4E"), ComVisible(true), ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces(typeof(INotificationActivationCallback))]
	public class NotificationActivator : NotificationActivatorAb
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

	    

        public override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
		{
            //Application.Current.Dispatcher.Invoke(delegate
            //{
                _logger.Info("OnActivated Invoked");
		        if (arguments.Length == 0) return;

			    // Parse the query string (using NuGet package QueryString.NET)
			    var args = QueryString.Parse(arguments);

			    // See what action is being requested 
			    switch (args["action"])
			    {
				    case "open":
                        _logger.Info("captured open");

				        Program.OpenAction();
                    // MessageBox.Show("Test", "Test");
                    //Process.Start("https://www.brave.com/");
                    //new Thread(() =>
                    //{
                    //    Thread.CurrentThread.IsBackground = true;
                    //    /* run your code here */
                    //    //Console.WriteLine("Hello, world");
                    //    Process.Start("https://www.brave.com/");


                    //}).Start();


                    //            try
                    //{

                    //    StartPage();
                    //            }
                    //catch (Exception ex)
                    //{
                    //    _logger.Error(ex, "Could not open page");
                    //   // throw;
                    //}

                    _logger.Info("end open event");
                        break;

				    // Background: Send a like
				    case "close":
					    _logger.Info("captured dismiss");

					    break;
				    default:
					    break;
			    }
            //});
        }

	    private void OpenWebPage()
	    {
	        Process.Start("https://google.com");
	    }

	    public static bool IsValidUri(string uri)
	    {
	        if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
	            return false;
	        Uri tmp;
	        if (!Uri.TryCreate(uri, UriKind.Absolute, out tmp))
	            return false;
	        return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps;
	    }

	    public static bool OpenUri(string uri)
	    {
	        if (!IsValidUri(uri))
	            return false;
	        System.Diagnostics.Process.Start(uri);
	        return true;
	    }

	    public async void StartPage()
	    {
	        var httpClient = new HttpClient();
	        var request = "https://www.google.com";
	        await httpClient.GetAsync(request);
            
        }
    }
}