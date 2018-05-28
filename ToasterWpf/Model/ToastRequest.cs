﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ToasterWpf.Model
{
	/// <summary>
	/// Toast request container
	/// </summary>
	[DataContract]
	public class ToastRequest
	{
	    public ToastRequest()
	    { }

        #region Public Property

        /// <summary>
        /// Toast title (optional)
        /// </summary>
        [DataMember]
		public string ToastTitle { get; set; }

		/// <summary>
		/// Toast body (required for toast)
		/// </summary>
		[DataMember]
		public string ToastBody
		{
			get { return ToastBodyList?[0]; }
			set { ToastBodyList = new string[] { value }; }
		}

		/// <summary>
		/// Toast body list (optional)
		/// </summary>
		/// <remarks>If specified, toast body will be substituted by this list.</remarks>
		[DataMember]
		public IList<string> ToastBodyList
		{
			get { return _toastBodyList; }
			set { _toastBodyList = value?.Where(x => !string.IsNullOrWhiteSpace(x)).ToList(); }
		}
		private IList<string> _toastBodyList;

		/// <summary>
		/// Logo image file path of toast (optional)
		/// </summary>
		/// <remarks>
		/// This file path must be in the following form:
		/// "file:///" + full file path
		/// </remarks>
		[DataMember]
		public string ToastLogoFilePath { get; set; }

		/// <summary>
		/// Audio type of toast (optional)
		/// </summary>
		[DataMember]
		public ToastAudioEnum ToastAudioEnum { get; set; }

		/// <summary>
		/// XML representation of toast (optional)
		/// </summary>
		/// <remarks>If specified, this XML will be used for a toast as it is. The other toast elements
		/// will be ignored.</remarks>
		[DataMember]
		public string ToastXml { get; set; }

		/// <summary>
		/// Shortcut file name to be installed in Start menu (required for shortcut)
		/// </summary>
		[DataMember]
		public string ShortcutFileName { get; set; }

		/// <summary>
		/// Target file path of shortcut (required for shortcut)
		/// </summary>
		[DataMember]
		public string ShortcutTargetFilePath { get; set; }

		/// <summary>
		/// Arguments of shortcut (optional)
		/// </summary>
		[DataMember]
		public string ShortcutArguments { get; set; }

		/// <summary>
		/// Comment of shortcut (optional)
		/// </summary>
		[DataMember]
		public string ShortcutComment { get; set; }

		/// <summary>
		/// Working folder of shortcut (optional)
		/// </summary>
		[DataMember]
		public string ShortcutWorkingFolder { get; set; }

		/// <summary>
		/// Window state of shortcut (optional)
		/// </summary>
		[DataMember]
		public ShortcutWindowState ShortcutWindowState { get; set; }

		/// <summary>
		/// Icon file path of shortcut (optional)
		/// </summary>
		/// <remarks>If not specified, target file path will be used.</remarks>
		[DataMember]
		public string ShortcutIconFilePath
		{
			get
			{
				return !string.IsNullOrWhiteSpace(_shortcutIconFilePath)
					? _shortcutIconFilePath
					: ShortcutTargetFilePath;
			}
			set { _shortcutIconFilePath = value; }
		}
		private string _shortcutIconFilePath;

		/// <summary>
		/// AppUserModelID of application (required)
		/// </summary>
		/// <remarks>
		/// An AppUserModelID must be in the following form:
		/// CompanyName.ProductName.SubProduct.VersionInformation
		/// It can have no more than 128 characters and cannot contain spaces. Each section should be
		/// camel-cased. CompanyName and ProductName should always be used, while SubProduct and
		/// VersionInformation are optional.
		/// </remarks>
		[DataMember]
		public string AppId { get; set; }

		/// <summary>
		/// AppUserModelToastActivatorCLSID of application (optional, for Action Center of Windows 10)
		/// </summary>
		/// <remarks>This CLSID is necessary for an application to be started by COM.</remarks>
		[DataMember]
		public Guid ActivatorId { get; set; }

		/// <summary>
		/// Waiting duration before showing a toast after the shortcut file is installed (optional)
		/// </summary>
		[DataMember]
		public TimeSpan WaitingDuration { get; set; }

		#endregion

		#region Internal Property

		//internal bool IsShortcutValid =>
		//	!string.IsNullOrWhiteSpace(AppId) &&
		//	!string.IsNullOrWhiteSpace(ShortcutFileName) &&
		//	!string.IsNullOrWhiteSpace(ShortcutTargetFilePath);

		internal bool IsToastValid =>
			!string.IsNullOrWhiteSpace(AppId) &&
			((ToastBodyList?.Any()).GetValueOrDefault() ||
			 !string.IsNullOrWhiteSpace(ToastXml));

		#endregion






		
	}
}