using ServiceHub.Contracts;
using System.Reflection;

namespace ServiceHub.Core
{
    public sealed class ServiceHub
    {
        private static readonly Lazy<ServiceHub> _instance = new(() => new ServiceHub());
        public static ServiceHub Instance => _instance.Value;

        private readonly List<IServiceModule> _modules = new();
        private readonly CancellationTokenSource _cts = new();

        private ServiceHub() { }

        public void LoadModules(string folderPath, IServiceContext context)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            foreach (var folder in (new DirectoryInfo(folderPath)).GetDirectories())

            foreach (var file in Directory.GetFiles(folder.FullName, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    foreach (var type in assembly.GetTypes())
                    {
                        if (!typeof(IServiceModule).IsAssignableFrom(type) || type.IsAbstract)
                            continue;

                        var module = (IServiceModule)Activator.CreateInstance(type);
                        module.Initialize(context);
                        _modules.Add(module);
                        context.Log($"Loaded module: {module.Name}");
                    }
                }
                catch (Exception ex)
                {
                    context.Log($"Failed to load {file}: {ex.Message}");
                }
            }
        }

        public void Start()
        {
            foreach (var module in _modules)
            {
                Task.Run(() => module.StartAsync(_cts.Token));
            }
        }

        public void Stop()
        {
            _cts.Cancel();
            foreach (var module in _modules)
            {
                try { module.Stop(); } catch { }
            }
        }

        public IReadOnlyList<IServiceModule> Modules => _modules.AsReadOnly();
    }
}