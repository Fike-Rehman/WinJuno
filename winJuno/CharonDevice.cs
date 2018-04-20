using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CTS.winJuno
{
    public class CharonDevice
    {
        private ICharonSettings _settings;

        #region singleton

        private static CharonDevice instance;

        protected CharonDevice() : this(new ConfigSettings())
        {


        }

        protected CharonDevice(ICharonSettings settings)
        {
            _settings = settings;

        }

        public static CharonDevice Instance() => instance ?? (instance = new CharonDevice());

        #endregion

        // start the process that talks to the Charon board
        public void Run(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                // send a Ping request:

                Task<string> pingTask = PingAsync();
            }
        }


        private async Task<string> PingAsync()
        {
            var pingResponse = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_settings.CharonDeviceIPAddress);
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
