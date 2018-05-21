namespace Toaster
{
    public class ToastModel
    {
        public string Title { get; set; }
        public string Body { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public bool Silent { get; set; }

    }
}