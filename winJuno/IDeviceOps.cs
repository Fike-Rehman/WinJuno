using System.Threading;
using System.Threading.Tasks;

namespace CTS.WinJuno
{
    public interface IDeviceOps
    {
        Task<PingResult> DevicePingAsync(string deviceIp, CancellationToken ct);

        void StartPingRoutine();
    }
}
