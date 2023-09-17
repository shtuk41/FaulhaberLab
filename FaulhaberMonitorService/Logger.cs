using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;

namespace FaulhaberMonitorService
{
    public class Logger
    {
        #region fields
        private static ILog log = LogManager.GetLogger("FaulhaberMonitor");

        #endregion

        /// <summary>
        /// log directory is configured in options.xml
        /// </summary>
        public static void ConfigureLogger()
        {
            string logDirectory = Options.LogDirectory;

            if (!System.IO.Directory.Exists(logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(logDirectory);
                }
                catch (IOException e)
                {
                    Console.WriteLine("IOException: " + e.Message);
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            string time = DateTime.Now.ToString("dddd_dd_MMMM_yyyy_HH_mm_ss");

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = false;
            roller.File = logDirectory + string.Format(@"\FaulhaberLog_{0}.log", time);
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "1GB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }

        /// <summary>
        /// this methods will log a message to file and print it to console
        /// </summary>
        /// <param name="msg">message to print</param>
        public static void PrintLog(string msg)
        {
            Console.WriteLine(msg);
            log.Info(msg);

        }
    }
}
