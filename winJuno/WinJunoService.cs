using System.Threading;
using System.Speech.Synthesis;
using System.Reflection;

namespace CTS.winJuno
{
    public class WinJunoService
    {
        private static readonly log4net.ILog _logger =
                 log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly SpeechSynthesizer speechSynthesizer;



        public WinJunoService()
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();
        }


        public void Start()
        {
            _logger.Debug("Windows Juno Service Started successfully!");

            speechSynthesizer.Speak("Windows Juno Service Started successfully!");   

            // init charon Device
        }

        public void Stop()
        {
            _logger.Info("Elara Service Stop requested!");

            int n = 3;
            while (n > 0)
            {
                _logger.InfoFormat($"Stoping Elara Service in {n} seconds...");
                n--;
                Thread.Sleep(1000);
            }

        }

    }
}

