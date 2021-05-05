using System;
using System.IO;


namespace WPFUI
{
    public abstract class LogBase
    {
        /*
            Abstract class 
        */
        public abstract void Log(string Message);
    }

    public class Logger : LogBase
    {
        /*
            Simple Logs saved to the File Los.txt.
        */
        private string CurrentDirectory { get; set; }
        private string FileName { get; set; }
        private string FilePath { get; set; }

        public Logger()
        {
            CurrentDirectory = Directory.GetCurrentDirectory();
            FileName = $"Log.txt";
            FilePath = $"{ CurrentDirectory }/{ FileName }";
        }

        public override void Log(string Message)
        {
            using (System.IO.StreamWriter w = System.IO.File.AppendText(this.FilePath)) {

                w.Write($"\r\nLog Entry at ");
                w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()} : {Message}");
                w.WriteLine($"-------------------------------------------------------------------------");
            
            }
        }
    }
}