# Toaster
The purpose of this project was to create a way to pop interactive toasts from the command line. I used a WPF application with startup arguments instead of a console application because it is necessary to keep the application open long enough to respond to events of COM activation.

## Usage

-t [string] Toast Title </br>
-b [string] Toast Body </br>
-p [string] Logo Image (Windows Path) </br>
-silent (If included no audio will be played)

## Interactive Components
The activator enables the toast to respond to events and is registered with the OS on startup.  Currently the buttons are defined in ToastXML.cs  The NotificationActivator class can be modified to respond to the button clicks as needed. 

It is modeled after this sample:

* [WindowsNotifications/desktop-toasts][2]

## Shortcut
The application will install a shortcut in the windows startup menu.  This is required for COM activation.  I used the approach from this well written project:

* [emoacht/DesktopToast][1]

## License

 - MIT License

[1]: https://github.com/emoacht/DesktopToast
[2]: https://github.com/WindowsNotifications/desktop-toasts