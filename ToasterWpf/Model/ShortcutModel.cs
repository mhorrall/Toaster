using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToasterWpf.Helper;

namespace ToasterWpf.Model
{
    public class ShortcutModel
    {
        /// <summary>
		/// Shortcut file name to be installed in Start menu (required for shortcut)
		/// </summary>
        public string ShortcutFileName { get; set; }

        /// <summary>
        /// Target file path of shortcut (required for shortcut)
        /// </summary>
        public string ShortcutTargetFilePath { get; set; }

        /// <summary>
        /// Arguments of shortcut (optional)
        /// </summary>
        public string ShortcutArguments { get; set; }

        /// <summary>
        /// Comment of shortcut (optional)
        /// </summary>
        public string ShortcutComment { get; set; }

        /// <summary>
        /// Working folder of shortcut (optional)
        /// </summary>
        public string ShortcutWorkingFolder { get; set; }

        /// <summary>
        /// Window state of shortcut (optional)
        /// </summary>
        public ShortcutWindowState ShortcutWindowState { get; set; }

        /// <summary>
        /// Icon file path of shortcut (optional)
        /// </summary>
        /// <remarks>If not specified, target file path will be used.</remarks>
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
        public string AppId { get; set; }

        /// <summary>
        /// AppUserModelToastActivatorCLSID of application (optional, for Action Center of Windows 10)
        /// </summary>
        /// <remarks>This CLSID is necessary for an application to be started by COM.</remarks>
        public Guid ActivatorId { get; set; }

        /// <summary>
        /// Waiting duration before showing a toast after the shortcut file is installed (optional)
        /// </summary>

        public TimeSpan WaitingDuration { get; set; }
    }
}
