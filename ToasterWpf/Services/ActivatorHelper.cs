using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using ToasterWpf.Helper;

namespace ToasterWpf.Services
{
	public  class ActivatorHelper
	{
		private static int? _cookie;

		/// <summary>
		/// Registers the activator type as a COM server client so that Windows can launch your activator.
		/// </summary>
		/// <typeparam name="T">Your implementation of NotificationActivator. Must have GUID and ComVisible attributes on class.</typeparam>
		public static void RegisterActivator<T>() where T : NotificationActivatorBase
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
			//_action = null;
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

		public static void CheckArgument(Type activatorType)
		{
			if (activatorType == null)
				throw new ArgumentNullException(nameof(activatorType));
		}

	}
}
