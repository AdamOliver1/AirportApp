using System;
using System.IO;

namespace Server.ErrorHandling
{

    public static class ExceptionLogger
    {
        private static string errorLineNum, errormsg, extype, errorLocation,source;

        public static void LoggExceptionToText(this Exception ex)
        {

            var line = Environment.NewLine + Environment.NewLine;

            errorLineNum = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            source = ex.Source;
               errorLocation = ex.Message.ToString();

            try
            {
                string filepath = Directory.GetCurrentDirectory() + "\\Loggs";

                if (!Directory.Exists(filepath))
                    Directory.CreateDirectory(filepath);
                filepath = filepath + "\\" + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                if (!File.Exists(filepath))
                    File.Create(filepath).Dispose();

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Souce :" + source + "Error Line Number :" + " " + errorLineNum + line + "Error Message:" + " " + errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + errorLocation + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    sw.WriteLine("-------------------------------------------------------------------------------------");
                    sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}
