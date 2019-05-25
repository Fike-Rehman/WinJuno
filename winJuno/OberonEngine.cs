using CTS.Common.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CTS.WinJuno
{
    public class OberonEngine
    {
        private static readonly log4net.ILog _logger =
                 log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private List<OberonDevice> _oberonDevices;

        private DateTime _sunriseToday;
        private DateTime _sunsetToday;

        public OberonEngine()
        {
            _oberonDevices = new List<OberonDevice>();
        }

        public void Run(CancellationToken cToken)
        {
            // Begin Oberon Activities

            // Get the sunrise/sunset times 
            SunTimes.GetSunTimes(out _sunriseToday, out _sunsetToday);

            // See how many Oberon deivces we have in the system:
            LoadDevices();

            // start a Ping task for each of the devices found:
            if (_oberonDevices.Count > 0)
            {
                _oberonDevices.ForEach(device =>
                {
                    // start up a ping task:
                    Task<string> pingTask = PingAsync(device.IpAddress);
                });
            }


        }


        private void LoadDevices()
        {
            using (StreamReader file = File.OpenText("OberonDevices.json"))
            {
                // var serializer = new JsonSerializer();
                string jsonString = file.ReadToEnd();
                _oberonDevices = JsonConvert.DeserializeObject<List<OberonDevice>>(jsonString);

                _logger.Info($"Found {_oberonDevices.Count} Oberon devices defined in the system!");
            }
        }

        private async Task<string> PingAsync(string deviceIp)
        {
            var pingResponse = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://{deviceIp}/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                client.Timeout = TimeSpan.FromMilliseconds(10000);

                try
                {
                    var response = await client.GetAsync("Ping");


                    if (response.IsSuccessStatusCode)
                    {
                        pingResponse = "Success";
                    }

                }
                catch (Exception x)
                {
                    // the request takes longer than 10 secs, it is timed out
                    pingResponse = x.Message;
                }

                return pingResponse;
            }
        }
    }
}
