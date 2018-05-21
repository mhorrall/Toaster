using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using DesktopToast;
using Microsoft.QueryStringDotNET;
using NotificationsExtensions;
using NotificationsExtensions.Toasts;


namespace Toaster
{
    public class SendToastService
    {
        private const string MessageId = "Message";

        public void ShowInteractiveToast(ToastModel toastModel, string AumId)
        {
            var toastXml = new ToastXml(toastModel);

            var request = new ToastRequest
            {
                ToastXml = toastXml.GetInteractiveToastXml(),
                ShortcutFileName = AumId + ".lnk",
                ShortcutTargetFilePath = Assembly.GetExecutingAssembly().Location,
                AppId = AumId,
                ActivatorId = typeof(NotificationActivator).GUID
            };

            var result = ToastManager.ShowAsync(request);
            result.Wait();
        }

        private string ComposeInteractiveToast()
        {
            var toastVisual = new ToastVisual
            {
                BindingGeneric = new ToastBindingGeneric
                {
                    Children =
                    {
                        new AdaptiveText { Text = "DesktopToast Console Sample" }, // Title
                        new AdaptiveText { Text = "This is an interactive toast test." }, // Body
                    },
                    AppLogoOverride = new ToastGenericAppLogo
                    {
                        //Source = string.Format("file:///{0}", Path.GetFullPath("Resources/toast128.png")),
                        Source = "C:\\Users\\matt.horrall\\Pictures\\pass.png",
                        AlternateText = "Logo"
                    }
                }
            };
            var toastAction = new ToastActionsCustom
            {
                Inputs =
                {
                    new ToastTextBox(id: MessageId) { PlaceholderContent = "Input a message" }
                },
                //Buttons =
                //{
                //	new ToastButton(content: "Reply", arguments: "action=Replied") { ActivationType = ToastActivationType.Background },
                //	new ToastButton(content: "Ignore", arguments: "action=Ignored")
                //}
                Buttons =
                {
                    // Note that there's no reason to specify background activation, since our COM
                    // activator decides whether to process in background or launch foreground window
                    new ToastButton("Reply", new QueryString()
                    {
                        { "action", "reply" }
                    }.ToString()),

                    new ToastButton("Ignore", new QueryString()
                    {
                        { "action", "ignore" }
                    }.ToString()),

                }

            };
            var toastContent = new ToastContent
            {
                Visual = toastVisual,
                Actions = toastAction,
                Duration = ToastDuration.Long,
                Audio = new NotificationsExtensions.Toasts.ToastAudio
                {
                    Loop = true,
                    Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm4")
                }
            };

            return toastContent.GetContent();
        }
    }
}