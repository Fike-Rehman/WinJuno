using System;
using System.Configuration;
using System.Globalization;

namespace CTS.winJuno
{
    public class ConfigSettings : ICharonSettings
    {
        private const string CharonDeviceIPAddressKey = "CharonDeviceIPAddress";

        private const string CharonDCRelayOnTimeOffsetKey = "CharonDCRelayOnTimeOffset";

        private const string CharonDCRelayOffTimeKey = "CharonDCRelayOffTime";

        private const string CharonACRelayOnTimeOffsetKey = "CharonACRelayOnTimeOffset";

        private const string CharonACRelayOffTimeKey = "CharonACRelayOffTime";

        private const string LocationLatitudeKey = "LocationLatitude";

        private const string LocationLongitudeKey = "LocationLongitude";


        /// <summary>
        /// IP address of the Charon Arduino board
        /// </summary>
        public string CharonDeviceIPAddress =>
            ConfigurationManager.AppSettings[CharonDeviceIPAddressKey] ?? "http://192.168.0.200/";

        /// <summary>
        /// DC relay On Time offset from Sunset time in minutes
        /// </summary>
        public int CharonDCRelayOnTimeOffset => int.TryParse(ConfigurationManager.AppSettings[CharonDCRelayOnTimeOffsetKey], out var myVal)
            ? myVal
            : 60;

        /// <summary>
        /// Charon DC relay off time (make sure this remains greater than On Time)
        /// </summary>
        public DateTime CharonDCRelayOffTime()
        {
            try
            {
                return DateTime.ParseExact(ConfigurationManager.AppSettings[CharonDCRelayOffTimeKey], "hh:mm",
                                           new CultureInfo("en-us"));
            }
            catch (Exception)
            {
                // put this is a log file
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Charon AC relay On time offset from Sunset in minutes
        /// </summary>
        public int CharonACRelayOnTimeOffset => int.TryParse(ConfigurationManager.AppSettings[CharonACRelayOnTimeOffsetKey], out var myVal)
            ? myVal
            : 60;

        /// <summary>
        /// Charon AC relay off time (make sure this remains greater than the On Time)
        /// </summary>
        public DateTime CharonACRelayOffTime()
        {
            try
            {
                return DateTime.ParseExact(ConfigurationManager.AppSettings[CharonACRelayOffTimeKey], "hh:mm",
                    new CultureInfo("en-us"));
            }
            catch (Exception)
            {
                // put this is a log file
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Latitude of the current location (for getting sun times)
        /// </summary>
        public double LocationLatitude =>
            double.TryParse(ConfigurationManager.AppSettings[LocationLatitudeKey], out var myVal) ? myVal : 44.98;

        /// <summary>
        /// Longitude of the current location (for getting sun times)
        /// </summary>
        public double LocationLongitude =>
            double.TryParse(ConfigurationManager.AppSettings[LocationLongitudeKey], out var myVal) ? myVal : -93.26;

    }
}
