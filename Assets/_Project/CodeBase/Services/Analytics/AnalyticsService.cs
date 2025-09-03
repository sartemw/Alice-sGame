using Io.AppMetrica;

namespace _Project.CodeBase.Services.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        public void Init()
        {
            AppMetricaActivator.Init();
        }

        public void Send(string message)
        {
            AppMetrica.ReportEvent(message);
        }
        
        public void SendWindow(string message)
        {
            AppMetrica.ReportEvent(message);
        }

        public void SendBuffer()
        {
            AppMetrica.SendEventsBuffer();
        }
        
        private void OnApplicationQuit()
        {
            
        }
    }
}