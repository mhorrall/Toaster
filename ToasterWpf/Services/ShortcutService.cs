using System;
using System.IO;
using System.Threading.Tasks;
using ToasterWpf.Helper;
using ToasterWpf.Model;

namespace ToasterWpf.Services
{
    public class ShortcutService
    {
        /// <summary>
        /// Waiting duration before showing a shortcutModel after the shortcut file is installed
        /// </summary>
        /// <remarks>It seems that roughly 3 seconds are required.</remarks>
        private static readonly TimeSpan _waitingDuration = TimeSpan.FromSeconds(3);

        /// <summary>
        /// Checks and installs a shortcut file in Start menu.
        /// </summary>
        /// <param name="shortcutModel">Toast shortcutModel</param>
        public static void CheckInstallShortcut(ShortcutModel shortcutModel)
        {
            var shortcutFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), // Not CommonStartMenu
                "Programs",
                shortcutModel.ShortcutFileName);

            var shortcut = new Shortcut();

            if (!shortcut.CheckShortcut(
                shortcutPath: shortcutFilePath,
                targetPath: shortcutModel.ShortcutTargetFilePath,
                arguments: shortcutModel.ShortcutArguments,
                comment: shortcutModel.ShortcutComment,
                workingFolder: shortcutModel.ShortcutWorkingFolder,
                windowState: shortcutModel.ShortcutWindowState,
                iconPath: shortcutModel.ShortcutIconFilePath,
                appId: shortcutModel.AppId,
                activatorId: shortcutModel.ActivatorId))
            {
                shortcut.InstallShortcut(
                    shortcutPath: shortcutFilePath,
                    targetPath: shortcutModel.ShortcutTargetFilePath,
                    arguments: shortcutModel.ShortcutArguments,
                    comment: shortcutModel.ShortcutComment,
                    workingFolder: shortcutModel.ShortcutWorkingFolder,
                    windowState: shortcutModel.ShortcutWindowState,
                    iconPath: shortcutModel.ShortcutIconFilePath,
                    appId: shortcutModel.AppId,
                    activatorId: shortcutModel.ActivatorId);

                System.Threading.Thread.Sleep(_waitingDuration);

                //await Task.Delay(_waitingDuration);
            }
        }
    }
}
