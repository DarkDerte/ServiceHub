using ServiceHub.Contracts.Enums;
using ServiceHub.Contracts.Interfaces;

namespace ServiceHub.Core.Model
{
    internal sealed class ServiceInstance
    {
        private string _serviceName;
        private IServiceModule? _serviceModule { get; set; }
        private IConfigContext _config { get; set; }
        private ILogContext _log { get; set; }
        public ServiceState State => _serviceModule?.State() ?? ServiceState.Error;

        public string Name => _serviceName;

        public void StartAsync(CancellationToken ct) => _serviceModule?.StartAsync(ct);

        public void Stop(CancellationToken ct) => _serviceModule?.Stop();

        public ServiceInstance(ILogContext log, IConfigContext config, Type type) 
        {
            _config = config;
            _log = log;

            _serviceName = config.Get("name") ?? string.Empty;
            if (string.IsNullOrEmpty(_serviceName))
                throw new ArgumentNullException("name");

            IConfigContext? contextValue = config.GetConfig("parameters");
            if (contextValue == null)
                throw new ArgumentNullException("parameters");

            try
            {
                if (!typeof(IServiceModule).IsAssignableFrom(type) || type.IsAbstract)
                    throw new Exception($"The {type.Name} Module is not valid" );

                _serviceModule = (IServiceModule)Activator.CreateInstance(type);
                _serviceModule?.Initialize(log, contextValue);
            }
            catch (Exception ex)
            {
                _log.Error($"Error : {ex.Message}");
                throw;
            }
        }
    }
}
