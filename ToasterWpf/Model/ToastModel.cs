using System;
using System.Runtime.Serialization;

namespace ToasterWpf.Model
{
    public class ToastModel
    {
        public string Title { get; set; }
        public string Body { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public bool Silent { get; set; }

        // Long (25s) or Short (7s)
        public string Duration { get; set; } = "";

       

    }
}