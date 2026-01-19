namespace ServiceHub.Contracts
{
    public interface IServiceModule
    {
        string Name { get; }
        void Initialize(IServiceContext context);
        Task StartAsync(CancellationToken token);
        void Stop();
    }
}
