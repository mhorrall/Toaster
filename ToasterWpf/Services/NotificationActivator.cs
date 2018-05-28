using System;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.QueryStringDotNET;
using NLog;

namespace ToasterWpf.Services
{
	/// <summary>
	/// Inherited class of notification activator (for Action Center of Windows 10)
	/// </summary>
	/// <remarks>The CLSID of this class must be unique for each application.</remarks>
	[Guid("182a25aa-6bcb-4cee-b13e-f9a727ddeb48"), ComVisible(true), ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces(typeof(INotificationActivationCallback))]
	public class NotificationActivator : NotificationActivatorBase
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public override void OnActivated(string arguments, NotificationUserInput userInput, string appUserModelId)
		{
            Application.Current.Dispatcher.Invoke(delegate
            {
                _logger.Info("OnActivated Invoked");
		        if (arguments.Length == 0) return;

			    // Parse the query string (using NuGet package QueryString.NET)
			    var args = QueryString.Parse(arguments);

			    // See what action is being requested 
			    switch (args["action"])
			    {
				    case "open":
                        _logger.Info("captured open");
				        OpenUri("https://www.brave.com/");
                        break;
				    case "close":
					    _logger.Info("captured dismiss");

					    break;
				    default:
					    break;
			    }
                Application.Current.Shutdown();
            });
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
    }
}