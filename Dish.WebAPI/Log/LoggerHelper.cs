using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Dish.WebAPI.Log
{
    public class LoggerHelper
    {
        private static LoggerHelper logger;
        private static string fileFolder;
        private static string fileName;
        private static StreamWriter writer;
        private static string strLock = string.Empty;

        public static LoggerHelper Instance
        {
            get
            {
                if (LoggerHelper.logger == null)
                {
                    lock (LoggerHelper.strLock)
                    {
                        if (LoggerHelper.logger == null)
                        {
                            LoggerHelper.logger = new LoggerHelper();
                        }
                    }
                }
                return LoggerHelper.logger;
            }
        }

        public static string FileName { get { return LoggerHelper.fileName; } }
        public string Text { get; private set; }

        private LoggerHelper()
        {
            // Create log file
            LoggerHelper.fileFolder = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), ConfigurationManager.AppSettings["LogFolder"]);
            LoggerHelper.fileName = DateTime.Now.ToString(LogFileNameFormat) + ".txt";
            DirectoryInfo di = new DirectoryInfo(LoggerHelper.fileFolder);
            if (!di.Exists)
            {
                di.Create();
            }

            // Create stream writer
            LoggerHelper.writer = new StreamWriter(Path.Combine(LoggerHelper.fileFolder, LoggerHelper.fileName), true);
        }

        public void WriteLine(string message)
        {
            string line = string.Format("{0}\t{1}", DateTime.Now.ToString(LogTextFormat, CultureInfo.InvariantCulture), message);
            Text += line + "\r\n";
            LoggerHelper.writer.WriteLine(line);
            LoggerHelper.writer.Flush();
        }

        public void EnterMethod(MethodInfo method)
        {
            WriteLine("Entering method: " + method.Name + " (" + string.Join(", ", method.GetParameters().Select(x => x.ToString())) + ")"
                + " called by type: " + method.DeclaringType);
        }

        public void LeaveMethod(MethodInfo method)
        {
            WriteLine("Leaving method: " + method.Name + "(" + string.Join(", ", method.GetParameters().Select(x => x.ToString())) + ")"
                + " called by type: " + method.DeclaringType);
        }

        public const string LogFileNameFormat = "yyyy-MM-dd-HH-mm-ss";
        public const string LogTextFormat = "yyyy-MM-dd HH:mm:ss.fff";

    }
}