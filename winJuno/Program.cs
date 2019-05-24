using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace CTS.WinJuno
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(config =>
            {
                    config.UseLog4Net("log4net.config", true);

                    config.Service<WinJunoService>(svr =>
                    {
                        svr.ConstructUsing(name => new WinJunoService());
                        svr.WhenStarted(es => es.Start());
                        svr.WhenStopped(es => es.Stop());
                    });

                    config.SetDescription("WinJuno Service hosts a gateway that enables communications between various Satellite device boards and the Internet");
                    config.SetDisplayName("Windows Juno Service");
                    config.SetServiceName("WinJunoService");

                    config.RunAsLocalSystem();
                    config.StartAutomatically();
            });
        }
    }
}
