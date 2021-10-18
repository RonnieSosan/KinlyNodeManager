using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinlyNodeManagerService.Utility
{
    public class Logger
    {
        static readonly string filename = string.Format("{0}Logs\\Node_Manager_Logs_{1:dd-MMM-yyyy}.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now);
        public Logger()
        {

        }
        public static void LogInfo(string MethodName, object message)
        {
            message = message.GetType() != typeof(string) ? JsonConvert.SerializeObject(message) : message;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
                sb.AppendLine("caller: " + MethodName + "\n: " + message);

                using System.IO.StreamWriter str = new System.IO.StreamWriter(filename, true);
                str.WriteLine(sb.ToString());

                //Task.Factory.StartNew(() => ReportErrorToServer(sb.ToString()));
            }
            catch { }
        }

        public static void LogError(Exception ex)
        {
            try
            {
                string message = GetExceptionMessages(ex);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Date and Time: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss"));
                sb.AppendLine("ErrorMessage: \n " + message);

                using System.IO.StreamWriter str = new System.IO.StreamWriter(filename, true);
                str.WriteLine(sb.ToString());

                //Task.Factory.StartNew(() => ReportErrorToServer(sb.ToString()));
            }
            catch { }
        }

        public static string GetExceptionMessages(Exception ex)
        {
            string ret = string.Empty;
            if (ex != null)
            {
                ret = ex.Message;
                if (ex.InnerException != null)
                    ret = ret + "\n" + GetExceptionMessages(ex.InnerException);
            }
            return ret;
        }
    }
}
