using System;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;

namespace ToasterWpf.Model
{
    public class ToastXml
    {
        private readonly ToastModel _toastModel;

        public ToastXml(ToastModel toastModel)
        {
            _toastModel = toastModel;
        }
        public string GetInteractiveToastXml()
        {
            var toastVisual = new ToastVisual
            {
                BindingGeneric = new ToastBindingGeneric
                {
                    Children =
                    {
                        new AdaptiveText { Text = _toastModel.Title }, // Title
                        new AdaptiveText { Text = _toastModel.Body }, // Body
                    },
                    AppLogoOverride = new ToastGenericAppLogo
                    {
                        Source = _toastModel.ImagePath,
                        AlternateText = "Logo"
                    }
                }
            };
            var toastAction = new ToastActionsCustom
            {
                //Inputs =
                //{
                //    new ToastTextBox(id: MessageId) { PlaceholderContent = "Input a message" }
                //},
                Buttons =
                {
                    // Note that there's no reason to specify background activation, since our COM
                    // activator decides whether to process in background or launch foreground window
                    new ToastButton("Open", new QueryString()
                    {
                        { "action", "open" }
                    }.ToString()),

                    new ToastButton("Close", new QueryString()
                    {
                        { "action", "dismiss" }
                    }.ToString()),

                }
            };

            var toastAudio = new ToastAudio();

            if (!_toastModel.Silent)
            {
                toastAudio = new ToastAudio
                {
                    Loop = true,
                    Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm4")
                };
            }

            var toastContent = new ToastContent
            {
                Visual = toastVisual,
                Actions = toastAction,
                Duration = ToastDuration.Long,
                Audio = toastAudio
            };

            return toastContent.GetContent();
        }
    }
}
