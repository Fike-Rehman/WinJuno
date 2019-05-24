using System;
using System.Collections.Generic;
using System.Reflection;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;


namespace CTS.WinJuno
{
    public class WinJunoService : IDisposable
    {
        private static readonly log4net.ILog _logger =
                 log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly SpeechSynthesizer speechSynthesizer;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly List<Task> _tasks = new List<Task>();



        private readonly OberonEngine oberonEngine;

        public WinJunoService()
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();

            oberonEngine = new OberonEngine();
        }


        public void Start()
        {

            if (_disposed) throw new ObjectDisposedException(nameof(cts));


            _logger.Info("Starting Juno Service. Please stand by...");

            speechSynthesizer.Speak("Starting Windows Juno Service. Please stand by...");

            var task = Task.Factory.StartNew(() => oberonEngine.Run(cts.Token));

            _logger.Info("Service Started successfully!");
        }

        public void Stop()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(cts));

            _logger.Info("Juno Service Stop requested!");
            speechSynthesizer.Speak("Stoping Juno Service. Please stand by...");

            int n = 3;
            while (n > 0)
            {
                _logger.InfoFormat($"Stoping Juno Service in {n} seconds...");
                n--;
                Thread.Sleep(1000);
            }

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
                cts?.Dispose();
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

