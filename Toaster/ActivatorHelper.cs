using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DesktopToast.Helper;
using Microsoft.Win32;

namespace DesktopToast.Wpf
{
	public  class ActivatorHelper
	{
		private static int? _cookie;
		private static Action<string, Dictionary<string, string>> _action;

		/// <summary>
		/// Register COM class type.
		/// </summary>
		/// <param name="activatorType">Notification activator type</param>
		/// <param name="action">Action to be invoked when Activate callback method is called</param>
		/// <remarks>Notification activator must inherit from this class.</remarks>
		//public static void RegisterComType(Type activatorType, Action<string, Dictionary<string, string>> action)
		//{
		//	NotificationHelper.CheckArgument(activatorType);

		//	if (!OsVersion.IsTenOrNewer)
		//		return;

		//	if (_cookie.HasValue)
		//		return;

		//	_cookie = new RegistrationServices().RegisterTypeForComClients(
		//		activatorType,
		//		RegistrationClassContext.LocalServer,
		//		RegistrationConnectionType.MultipleUse);

		//	_action = action;
		//}

		/// <summary>
		/// Registers the activator type as a COM server client so that Windows can launch your activator.
		/// </summary>
		/// <typeparam name="T">Your implementation of NotificationActivator. Must have GUID and ComVisible attributes on class.</typeparam>
		public static void RegisterActivator<T>() where T : NotificationActivatorAb
		{
			if (!OsVersion.IsTenOrNewer)
				return;

			if (_cookie.HasValue)
				return;

			// Register type
			var regService = new RegistrationServices();

			_cookie = regService.RegisterTypeForComClients(
				typeof(T),
				RegistrationClassContext.LocalServer,
				RegistrationConnectionType.MultipleUse);


		}

		/// <summary>
		/// Unregister COM class type.
		/// </summary>
		public static void UnregisterComType()
		{
			if (!_cookie.HasValue)
				return;

			new RegistrationServices().UnregisterTypeForComClients(_cookie.Value);
			_cookie = null;
			_action = null;
		}

		/// <summary>
		/// Register COM server in the registry.
		/// </summary>
		/// <param name="activatorType">Notification activator type</param>
		/// <param name="executablePath">Executable file path</param>
		/// <param name="arguments">Arguments</param>
		/// <remarks>If the application is not running, this executable file will be started by COM.</remarks>
		public static void RegisterComServer(Type activatorType, string executablePath, string arguments = null)
		{
			CheckArgument(activatorType);

			if (string.IsNullOrWhiteSpace(executablePath))
				throw new ArgumentNullException(nameof(executablePath));

			if (!OsVersion.IsTenOrNewer)
				return;

			var combinedPath = $@"""{executablePath}"" {arguments}";
			var keyName = $@"SOFTWARE\Classes\CLSID\{{{activatorType.GUID}}}\LocalServer32";
			using (var key = Registry.CurrentUser.OpenSubKey(keyName))
			{
				if (string.Equals(key?.GetValue(null) as string, combinedPath, StringComparison.OrdinalIgnoreCase))
					return;
			}
			using (var key = Registry.CurrentUser.CreateSubKey(keyName))
			{
				key.SetValue(null, combinedPath);
			}
		}
		public const string TOAST_ACTIVATED_LAUNCH_ARG = "-ToastActivated";
		private static void RegisterComServer<T>(String exePath) where T : NotificationActivatorAb
		{
			// We register the EXE to start up when the notification is activated
			string regString = $"SOFTWARE\\Classes\\CLSID\\{{{typeof(T).GUID}}}\\LocalServer32";
			var key = Registry.CurrentUser.CreateSubKey(regString);

			// Include a flag so we know this was a toast activation and should wait for COM to process
			// We also wrap EXE path in quotes for extra security
			key.SetValue(null, '"' + exePath + '"' + " " + TOAST_ACTIVATED_LAUNCH_ARG);
		}

		public static void CheckArgument(Type activatorType)
		{
			if (activatorType == null)
				throw new ArgumentNullException(nameof(activatorType));

			//if (!activatorType.IsSubclassOf(typeof(NotificationActivatorBase)))
			//throw new ArgumentException($"{nameof(activatorType)} must inherit from {nameof(NotificationActivatorBase)}.");
		}

	}
}
