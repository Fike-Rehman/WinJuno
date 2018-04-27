using System.Threading;
using System.Speech.Synthesis;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CTS.winJuno
{
    public class WinJunoService : IDisposable
    {
        private static readonly log4net.ILog _logger =
                 log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly SpeechSynthesizer speechSynthesizer;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly List<Task> _tasks = new List<Task>();

        private readonly List<IJunoDevice> _junoDevices;


        public WinJunoService()
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();

            _junoDevices = new List<IJunoDevice>
            {
                CharonDevice.Instance()
            };
        }


        public void Start()
        {

            if (_disposed) throw new ObjectDisposedException(nameof(_cts));


            _logger.Debug("Starting Windows Juno Service. Please stand by...");

            speechSynthesizer.Speak("Starting Windows Juno Service. Please stand by...");

            // Beign winJuno Activities

            

            // start actions and hold on to their tasks for cancellation on stop.
            foreach (var device in _junoDevices)
            {
                //_logger.Debug($"Starting task for {activity.GetType().Name}");
                // Pass the cancellation token to both the action (whether its used or not) as well as the StartNew()
                // the former to support cancellation if the action supports it, the latter to support cancellation
                // before the task starts up
                //var task = Task.Factory.StartNew(() => device.Run(_cts.Token), _cts.Token);
                //_tasks.Add(task);

                 Task.Run(() => device.Run(_cts.Token));
                // stagger starting, so that log messages don't overlap and to minimize concurrency and thread use.

                _logger.Debug("Started Charon device...");
                Thread.Sleep(100);
            }

            _logger.Info("Service Started successfully!");
        }

        public void Stop()
        {
            _logger.Info("Juno Service Stop requested!");

            int n = 3;
            while (n > 0)
            {
                _logger.InfoFormat($"Stoping Juno Service in {n} seconds...");
                n--;
                Thread.Sleep(1000);
            }

        }

        private void LaunchCharon()
        {

        }

        #region IDisposable Members

        /// <summary>
        /// Internal variable which checks if Dispose has already been called
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _cts?.Dispose();
            }

            _disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Call the private Dispose(bool) helper and indicate 
            // that we are explicitly disposing
            this.Dispose(true);

            // Tell the garbage collector that the object doesn't require any
            // cleanup when collected since Dispose was called explicitly.
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}

