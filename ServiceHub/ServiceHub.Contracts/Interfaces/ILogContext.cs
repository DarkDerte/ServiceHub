namespace ServiceHub.Contracts.Interfaces
{
    public interface ILogContext
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message);
    }
}
