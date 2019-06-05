using System.Threading.Tasks;

namespace CTS.WinJuno
{
    public interface IDeviceOps
    {
        Task<bool> DevicePingAsync(string deviceIp);

        void StartPingRoutine(OberonDevice device);
    }
}
