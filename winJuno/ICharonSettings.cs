using System;

namespace CTS.winJuno
{
    public interface ICharonSettings
    {
        /// <summary>
        /// IP address of the Charon Arduino board
        /// </summary>
        string CharonDeviceIPAddress { get; }

        /// <summary>
        /// DC relay On Time offset from Sunset time in minutes
        /// </summary>
        int CharonDCRelayOnTimeOffset { get; }

        /// <summary>
        /// Charon DC relay off time (make sure this remains greater than On Time)
        /// </summary>
        DateTime CharonDCRelayOffTime();

        /// <summary>
        /// Charon AC relay On time offset fro Sunset in minutes
        /// </summary>
        int CharonACRelayOnTimeOffset { get; }

        /// <summary>
        /// Charon AC relay off time (make sure this remains greater than the On Time)
        /// </summary>
        DateTime CharonACRelayOffTime();

        /// <summary>
        /// Latitude of the current location (for getting sun times)
        /// </summary>
        double LocationLatitude { get; }

        /// <summary>
        /// Longitude of the current location (for getting sun times)
        /// </summary>
        double LocationLongitude { get; }

    }
}
