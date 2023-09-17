using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FaulhaberMonitorService
{
    public class HttpHandler
    {
        #region fields

            private string address = @"http://localhost:8082";
            private string url = @"api/asis/v1/monitoring";
            private bool awaitResponse = true;
            private string jsonTextHeader = @"application/json";
       
        #endregion

        public HttpHandler(string address, string url, bool awaitResponse = true)
        {
            this.address = address;
            this.url = url;
            this.awaitResponse = awaitResponse;
        }

        public HttpHandler()
        {
            Logger.PrintLog("HttpHandler usind default configuration");
        }

        #region  methods

        public void UpdateStatus(int position, bool positionAlarm)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(address);

                string jsonStringToSend = JsonString(position, positionAlarm);

                StringContent content = new StringContent(jsonStringToSend, Encoding.UTF8, jsonTextHeader);

                Logger.PrintLog("UpdateStatus address: " + client.BaseAddress);
                Logger.PrintLog("UpdateStatus json: " + jsonStringToSend);

                if (awaitResponse)
                {
                    HttpResponseMessage message = client.PostAsync(url, content).Result;
                    Logger.PrintLog("UpdateStatus response: " + message.Content);
                }
                else
                {
                    client.PostAsync(url, content);
                }
            }
            catch (Exception e)
            {
                Logger.PrintLog("UpdateStatus exception: " + e.Message);
            }
        }

        #endregion

        #region functions


        private string JsonString(int position, bool positionAlarm)
        {
            string posValue = string.Format("\"value\": \"{0}\",", position);
            string alarmValue = string.Format("\"is_active\": {0}", positionAlarm ? "true" : "false");

            string ret = "{" +
                            "\"dataPoints\": " +
                            "[" +
                                "{" +
                                    "\"name\": \"APERTURE_POSITION\"," +
                                    posValue +
                                    "\"uom\": \"degThousand\"," +
                                    "\"resource_id\": \"APERTURE_POSITION\"" +
                                "}" +
                            "]," +
                            "\"alarms\":" +
                            "[" +
                                "{" +
                                "\"name\": \"APERTURE_MOTOR_ENCODER_ALARM\"," +
                                "\"resource_id\": \"APERTURE_MOTOR_ENCODER_ALARM\"," +
                                "\"data\": {}," +
                                alarmValue +
                                "}" +
                            "]" +
                        "}";

            return ret;
        }

        #endregion functions

    }
}
