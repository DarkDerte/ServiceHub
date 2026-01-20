using ServiceHub.Contracts.Enums;

namespace ServiceHub.Contracts.Interfaces
{
    public interface IServiceModule
    {
        string Name { get; }
        void Initialize(IServiceContext context);
        Task StartAsync(CancellationToken token);
        void Stop();
        ServiceState State();
    }
}
