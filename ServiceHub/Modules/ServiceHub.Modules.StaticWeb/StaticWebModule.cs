using EmbedIO;
using EmbedIO.Files;
using Microsoft.Extensions.FileProviders;
using ServiceHub.Contracts.Enums;
using ServiceHub.Contracts.Interfaces;

namespace ServiceHub.Modules.StaticWeb
{
    public class StaticWebModule : IServiceModule
    {
        private WebServer _server;
        private CancellationToken _token;

        public string Name => "StaticWebModule";

        public void Initialize(IServiceContext context)
        {
            context.Log($"{Name} initialized.");
        }

        public async Task StartAsync(CancellationToken token)
        {
            _token = token;

            var fileProvider = new PhysicalFileProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules", "WebModule", "wwwroot"));

            _server = new WebServer(o => o
                    .WithUrlPrefix("http://localhost:8080/")
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
