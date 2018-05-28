using System.Reflection;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using ToasterWpf.Model;

namespace ToasterWpf.Services
{
    public static class ToastService
    {
        public static void ShowInteractiveToast(ToastModel toastModel, string appId)
        {
            var toastXml = new ToastXml(toastModel);

            // Register shortcut
            var shortcut = new ShortcutModel
            {
                ShortcutFileName = appId + ".lnk",
                ShortcutTargetFilePath = Assembly.GetExecutingAssembly().Location,
                AppId = appId,
                ActivatorId = typeof(NotificationActivator).GUID
            };

            ShortcutService.CheckInstallShortcut(shortcut);

            // Create Xml document
            var document = new XmlDocument();
            document.LoadXml(toastXml.GetInteractiveToastXml());

            // Send toast
            var toast = new ToastNotification(document);
            ToastNotificationManager.CreateToastNotifier(appId).Show(toast);
        }
    }
}