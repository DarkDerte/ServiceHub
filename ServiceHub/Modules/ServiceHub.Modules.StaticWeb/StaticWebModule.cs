using EmbedIO;
using EmbedIO.Files;
using Microsoft.Extensions.FileProviders;
using ServiceHub.Contracts.Enums;
using ServiceHub.Contracts.Interfaces;

namespace ServiceHub.Modules.StaticWeb
{
    public class StaticWebModule : IServiceModule
    {
        private int _port = 8080;
        private string _directory = "wwwroot";

        private WebServer? _server;
        private CancellationToken _token;
        private ILogContext? _logContext;
        private IConfigContext? _configContext;

        public string Name => "StaticWebModule";

        public void Initialize(ILogContext log, IConfigContext config)
        {
            _logContext = log;
            _configContext = config;

            var port = _configContext.Get("port") ?? "8080";
            var root = _configContext.Get("directory") ?? "wwwroot";

            _port = int.Parse(port);
            _directory = root;

            _logContext.Info($"{Name} initialized.");
        }

        public async Task StartAsync(CancellationToken token)
        {
            _token = token;

            var fileProvider = new PhysicalFileProvider(_directory);

            _server = new WebServer(o => o
                    .WithUrlPrefix($"http://localhost:{_port}/")
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithLocalSessionManager()
                .WithModule(new FileModule("wwwroot", (EmbedIO.Files.IFileProvider)fileProvider));

            await _server.RunAsync(_token);
        }

        public ServiceState State()
        {
            return _server?.State switch { 
                WebServerState.Listening => ServiceState.Running,
                WebServerState.Loading => ServiceState.Running,
                WebServerState.Created => ServiceState.Running,
                WebServerState.Stopped => ServiceState.Stoped,
                _ => ServiceState.Stoped,
            };
        }

        public void Stop()
        {
            _server?.Dispose();
        }
    }
}
