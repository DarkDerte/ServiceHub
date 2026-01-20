using ServiceHub.Contracts.Enums;
using ServiceHub.Contracts.Interfaces;

namespace ServiceHub.Core.Model
{
    internal sealed class ServiceInstance
    {
        private string _serviceName;
        private IServiceModule? ServiceModule { get; set; }
        private IServiceContext Context { get; set; }
        public ServiceState State => ServiceModule?.State() ?? ServiceState.Error;

        public string Name => _serviceName;

        public void StartAsync(CancellationToken ct) => ServiceModule?.StartAsync(ct);

        public void Stop(CancellationToken ct) => ServiceModule?.Stop();

        public ServiceInstance(string name, Type type, IServiceContext context) 
        {
            Context = context;
            _serviceName = name;
            try
            {
                if (!typeof(IServiceModule).IsAssignableFrom(type) || type.IsAbstract)
                    throw new Exception($"The {type.Name} Module is not valid" );

                ServiceModule = (IServiceModule)Activator.CreateInstance(type);
                ServiceModule?.Initialize(context);
            }
            catch (Exception ex)
            {
                Context.Log($"Error : {ex.Message}");
                throw;
            }
        }
    }
}
