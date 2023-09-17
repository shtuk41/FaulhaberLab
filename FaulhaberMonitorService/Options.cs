using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FaulhaberMonitorService
{
    public class Options
    {
        #region properties
        /// <summary>
        /// Gets log directory from faulhaber options file
        ///  <logDirectory>C:/log/FaulhaberLog</logDirectory>
        /// </summary>
        public static string LogDirectory
        {
            get
            {
                string opt = ReadOptionFromFile("logDirectory");

                if (string.IsNullOrEmpty(opt))
                {
                    return @"C:\logs\FaulhaberLogs";
                }
                else
                {
                    return opt;
                }
            }
        }

        /// <summary>
        /// Gets update period from options file
        /// <updatePeriodSec>60</updatePeriodSec>
        /// </summary>
        public static int UpdatePeriodSec
        {
            get
            {
                string opt = ReadOptionFromFile("updatePeriodSec");
                int period = 60;

                if (string.IsNullOrEmpty(opt) || !int.TryParse(opt, out period))
                {
                    return 60;
                }
                else
                {
                    return period;
                }
            }
        }

        /// <summary>
        /// Gets port number from options file
        /// <port>COM3</port>
        /// </summary>
        public static string SerialPort
        {
            get
            {
                string opt = ReadOptionFromFile("port");

                if (string.IsNullOrEmpty(opt))
                {
                    return @"COM2";
                }
                else
                {
                    return opt;
                }
            }
        }

        /// <summary>
        /// Gets target position from options file
        /// <position>20162</position>
        /// </summary>
        public static int TargetPosition
        {
            get
            {
                string opt = ReadOptionFromFile("position");
                int position = 20162;

                if (string.IsNullOrEmpty(opt) || !int.TryParse(opt, out position))
                {
                    return 20162;
                }
                else
                {
                    return position;
                }
            }
        }

        /// <summary>
        /// Gets +/- tolerance for position from options file
        /// <tolerance>2016</tolerance>
        /// </summary>
        public static int TargetPositionTolerance
        {
            get
            {
                string opt = ReadOptionFromFile("tolerance");
                int tolerance = 2016;

                if (string.IsNullOrEmpty(opt) || !int.TryParse(opt, out tolerance))
                {
                    return 2016;
                }
                else
                {
                    return tolerance;
                }
            }
        }

        /// <summary>
        /// Gets server address from options file
        /// <address>http://localhost:8082</address>
        /// </summary>
        public static string Address
        {
            get
            {
                string opt = ReadOptionFromFile("address");

                if (string.IsNullOrEmpty(opt))
                {
                    return @"http://localhost:8082";
                }
                else
                {
                    return opt;
                }
            }
        }

        /// <summary>
        /// Gets server address from options file
        /// <url>api/asis/v1/monitoring</url>
        /// </summary>
        public static string Url
        {
            get
            {
                string opt = ReadOptionFromFile("url");

                if (string.IsNullOrEmpty(opt))
                {
                    return @"api/asis/v1/monitoring";
                }
                else
                {
                    return opt;
                }
            }
        }
        #endregion

        private static string ReadOptionFromFile(string option)
        {
            string fileName = "faulhaberoptions.xml";
            string retOption = string.Empty;

            if (File.Exists(fileName))
            {
                XDocument optionFile = XDocument.Load(fileName);

                IEnumerable<XElement> op = optionFile.Descendants(option);

                if (op.Count() > 0)
                {
                    retOption = op.First().Value;
                }
            }

            return retOption;
        }
    }
}
