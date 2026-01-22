using ServiceHub.Contracts.Enums;

namespace ServiceHub.Contracts.Interfaces
{
    public interface IServiceModule
    {
        string Name { get; }
        void Initialize(ILogContext log, IServiceContext config);
        Task StartAsync(CancellationToken token);
        void Stop();
        ServiceState State();
    }
}
