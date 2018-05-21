using System.Runtime.InteropServices;
using Windows.UI.Core;
using DesktopToast.Wpf;
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
			_logger.Info("OnActivated Invoked");
		    if (arguments.Length == 0) return;

			// Parse the query string (using NuGet package QueryString.NET)
			var args = QueryString.Parse(arguments);

			// See what action is being requested 
			switch (args["action"])
			{
				case "open":
					_logger.Info("captured open");

					break;

				// Background: Send a like
				case "dismiss":
					_logger.Info("captured dismiss");

					break;
				default:
					break;
			}
		}
	}
}