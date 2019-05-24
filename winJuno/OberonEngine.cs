using CTS.Common.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CTS.WinJuno
{
    public class OberonEngine
    {
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
        }


        private void LoadDevices()
        {
            using (StreamReader file = File.OpenText("OberonDevices.json"))
            {
               // var serializer = new JsonSerializer();
                string jsonString = file.ReadToEnd();
                _oberonDevices = JsonConvert.DeserializeObject<List<OberonDevice>>(jsonString);
                
            }
        }
    }
}
