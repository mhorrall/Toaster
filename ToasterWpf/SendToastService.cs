using System;
using System.Reflection;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using ToasterWpf.Model;


namespace ToasterWpf
{
    public class SendToastService
    {
        public void ShowInteractiveToast(ToastModel toastModel, string AumId)
        {
            var toastXml = new ToastXml(toastModel);

            //var request = new ToastRequest
            //{
            //    ToastXml = toastXml.GetInteractiveToastXml(),
            //    ShortcutFileName = AumId + ".lnk",
            //    ShortcutTargetFilePath = Assembly.GetExecutingAssembly().Location,
            //    AppId = AumId,
            //    ActivatorId = typeof(NotificationActivator).GUID
            //};

            // Register shortcut
            var shortcut = new ShortcutModel
            {
                ShortcutFileName = AumId + ".lnk",
                ShortcutTargetFilePath = Assembly.GetExecutingAssembly().Location,
                AppId = AumId,
                ActivatorId = typeof(NotificationActivator).GUID
            };

            ShortcutService.CheckInstallShortcut(shortcut).Wait();





            //var testXml = document.GetXml();
            //ToastNotificationManager.CreateToastNotifier(request.AppId).Show(toast);

            var document = new XmlDocument();
            document.LoadXml(toastXml.GetInteractiveToastXml());

            var toast = new ToastNotification(document);
            ToastNotificationManager.CreateToastNotifier(AumId).Show(toast);

            //var result = ToastManager.ShowAsync(request);
            //result.Wait();
        }

        
    }
}