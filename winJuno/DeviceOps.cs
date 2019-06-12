using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace CTS.WinJuno
{
    public class DeviceOps : IDeviceOps
    {
        private static readonly log4net.ILog _logger =
                 log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public async Task<bool> DevicePingAsync(string deviceIp)
        {
            var bSuccess = false;

            var n = 0;

            while (n < 3)
            {
                n++;

                var pingresponse = await PingAsync(deviceIp);

                if (pingresponse == "Success")
                {
                    bSuccess = true;
                    break;
                }


                if (n == 3)
                {
                    // already attempted 3 times and it failed every time.
                    bSuccess = false;
                    _logger.Error($"Device with Ip Address: {deviceIp} has failed to respond to repeated Ping requests");
                    _logger.Error("Please check this device and make sure that it is still Online");
                }
                else
                {
                    await Task.Delay(3000); // give it a 3 sec delay before trying again
                }
            }

            return bSuccess;
        }


        public void StartPingRoutine(OberonDevice device)
        {
            throw new NotImplementedException();
        }



        private async Task<string> PingAsync(string deviceIp)
        {
            var pingResponse = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://{deviceIp}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                client.Timeout = TimeSpan.FromMilliseconds(10000);

                try
                {
                    var response = await client.GetAsync("/ping");


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
