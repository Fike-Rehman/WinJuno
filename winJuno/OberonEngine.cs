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

        private Timer _pingTimer;

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

            if (_oberonDevices.Count > 0)
            {
                _oberonDevices.ForEach(device =>
                {
                    // Initialize the device by sending a Ping
                    Task<string> pingTask = PingAsync(device.IpAddress);

                    if (pingTask.Result != "Success")
                    {
                        _logger.Warn($"PING FAILED: A device with IP Address:{device.IpAddress} doesn't appear to be online!");
                        _logger.Warn("Removing the device from the device list");

                        _oberonDevices.Remove(device);
                    }
                    else
                    {
                        // Device initialization succeeded. We can continue with more operations:
                        // set up a timer that sends a ping asynchronously every minute:
                        var pingInterval = new TimeSpan(0, 0, 1, 0); // 1 minute  
                        _pingTimer = new Timer(OnPingTimer, device, pingInterval, Timeout.InfiniteTimeSpan);

                    }
                });
            }
            else
            {
                _logger.Warn("No devices found in the system!");
            }
        }


        private void LoadDevices()
        {
            try
            {
                using (StreamReader file = File.OpenText("OberonDevices.json"))
                {
                    // var serializer = new JsonSerializer();
                    string jsonString = file.ReadToEnd();
                    _oberonDevices = JsonConvert.DeserializeObject<List<OberonDevice>>(jsonString);

                    _logger.Info($"Found {_oberonDevices.Count} Oberon devices defined in the system!");
                }
            }
            catch (Exception x)
            {

                _logger.Error($"Error while reading Oberon Devices file: {x.Message}");
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

        private async void OnPingTimer(object device)
        {
            // send a ping asynchronously and reset the timer
            if (device is OberonDevice d)
            {
                var response = await PingAsync(d.IpAddress);

                if (response == "Success")
                {
                    var pingInterval = new TimeSpan(0, 0, 1, 0); // 1 minute
                    _pingTimer.Change(pingInterval, Timeout.InfiniteTimeSpan);
                }
            }
        }

    }
}
