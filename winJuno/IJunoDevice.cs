using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CTS.winJuno
{
    public interface IJunoDevice
    {
        void Run(CancellationToken token);
    }
}
