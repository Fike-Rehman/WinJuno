using System;

namespace CTS.WinJuno
{
    public class OberonDevice
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public string Location { get; set; }

        public TimeSpan OnTimeOffset { get; set; }

        public TimeSpan OffTime { get; set; }

    }
}
