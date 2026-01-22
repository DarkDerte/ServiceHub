using MQTTnet.Diagnostics.Logger;
using MQTTnet.Server;
using ServiceHub.Contracts.Enums;
using ServiceHub.Contracts.Interfaces;

namespace ServiceHub.Modules.MQTT
{
    public class MqttModule : IServiceModule
    {
        private MqttServer? _server = null;
        private ILogContext? _log = null;

        public string Name => "MqttModule";


        public void Initialize(ILogContext log, IServiceContext config)
        {
            _log = log;
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken token)
        {
            Stop();

            var logger = new MqttNetEventLogger();

            logger.LogMessagePublished += (o, e) =>
            {
                switch (e.LogMessage.Level)
                {
                    case MqttNetLogLevel.Error: _log?.Error(e.LogMessage.Message); break;
                    case MqttNetLogLevel.Warning: _log?.Warning(e.LogMessage.Message); break;
                    case MqttNetLogLevel.Info:
                    case MqttNetLogLevel.Verbose: _log?.Info(e.LogMessage.Message); break;
                }
            };

            _server = new MqttServerFactory(logger).CreateMqttServer(new MqttServerOptions());
            
            return _server.StartAsync();
        }

        public ServiceState State() => (_server?.IsStarted ?? false ) ? ServiceState.Running : ServiceState.Stoped;

        public void Stop() => _server?.StopAsync().Wait();
    }
}
