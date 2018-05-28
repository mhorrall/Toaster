using System.Reflection;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using ToasterWpf.Model;

namespace ToasterWpf.Services
{
    public class SendToastService
    {
        public void ShowInteractiveToast(ToastModel toastModel, string aumId)
        {
            var toastXml = new ToastXml(toastModel);

            // Register shortcut
            var shortcut = new ShortcutModel
            {
                ShortcutFileName = aumId + ".lnk",
                ShortcutTargetFilePath = Assembly.GetExecutingAssembly().Location,
                AppId = aumId,
                ActivatorId = typeof(NotificationActivator).GUID
            };

            ShortcutService.CheckInstallShortcut(shortcut).Wait();

            // Create Xml document
            var document = new XmlDocument();
            document.LoadXml(toastXml.GetInteractiveToastXml());

            // Send toast
            var toast = new ToastNotification(document);
            ToastNotificationManager.CreateToastNotifier(aumId).Show(toast);
        }

        
    }
}