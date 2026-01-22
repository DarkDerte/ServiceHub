using ServiceHub.Contracts.Interfaces;
using ServiceHub.Core.Model;
using System.Reflection;

namespace ServiceHub.Core
{
    public sealed class ServiceHub
    {
        #region Singleton
        
        private static readonly Lazy<ServiceHub> _instance = new(() => new ServiceHub());
        
        public static ServiceHub Instance => _instance.Value;

        private ServiceHub() { }

        #endregion

        #region Properties

        private Dictionary<string, Type> _availableModules = new ();

        private Dictionary<string, ServiceInstance> _modulesLoaded = new();

        private readonly CancellationTokenSource _cts = new();

        #endregion

        #region Public Methods

        public bool InitModule(ILogContext log, IServiceContext config)
        {
            var typeService = config.Get("type") ?? string.Empty;

            if (string.IsNullOrEmpty(typeService))
                throw new ArgumentNullException("type");

            if (!_availableModules.TryGetValue(typeService, out Type? type))
                return false;

            try {
                var instance = new ServiceInstance(log, config, type);
                _modulesLoaded.Add(instance.Name.ToLower().Trim(), instance);
            } 
            catch {
                return false;
            }
            return true;
        }

        public void Start(string name)
        {
            if(_modulesLoaded.TryGetValue(name.Trim().ToLower(), out ServiceInstance? instance))
                Task.Run(() => instance.StartAsync(_cts.Token));
        }

        public void Stop(string name)
        {
            if (_modulesLoaded.TryGetValue(name.Trim().ToLower(), out ServiceInstance? instance))
                Task.Run(() => { instance.Stop(_cts.Token); _modulesLoaded.Remove(name); });
        }

        public void StopAll()
        {
            _cts.Cancel();
            _modulesLoaded.ToList().ForEach((x) => {
                try { x.Value.Stop(_cts.Token); } catch { }
            });
        }

        #endregion

        public void LoadModules(ILogContext log, string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            foreach (var folder in (new DirectoryInfo(folderPath)).GetDirectories())
            {
                foreach (var file in Directory.GetFiles(folder.FullName, "*.dll"))
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(file);
                        foreach (var type in assembly.GetTypes())
                        {
                            if (!typeof(IServiceModule).IsAssignableFrom(type) || type.IsAbstract)
                                continue;

                            var name = ((IServiceModule?)Activator.CreateInstance(type))?.Name;
                            if (!string.IsNullOrWhiteSpace(name))
                            {
                                _availableModules.Add(name ?? string.Empty, type);
                                log.Info($"Loaded module: {name}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Warning($"Failed to load {file}: {ex.Message}");
                    }
                }
            }
        }

        public IReadOnlyList<string> Modules => _availableModules.Select(x=>x.Key).ToList().AsReadOnly();
        public IReadOnlyList<string> ServicesInitialized => _modulesLoaded.Keys.ToList().AsReadOnly();
    }
}