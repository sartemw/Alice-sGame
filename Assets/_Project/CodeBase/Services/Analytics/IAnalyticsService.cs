namespace _Project.CodeBase.Services.Analytics
{
    public interface IAnalyticsService: IService
    {
        void Init();
        void Send(string message);
        void SendWindow(string message);
        void SendBuffer();
    }
}