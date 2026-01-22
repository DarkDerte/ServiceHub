using ServiceHub.Core.Classes;

namespace ServiceHub.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new SimpleContext();
            var log = new LogContext();

            context.LoadJson(@"{
  ""services"": [
    {
      ""name"": ""WebStatic"",
      ""type"": ""StaticWebModule"",
      ""parameters"": {
          ""port"": ""8080"",
          ""root"": ""wwwroot""
      }
    },
    {
      ""name"": ""MqttIot"",
      ""type"": ""MqttModule"",
      ""parameters"": { }
    }
  ]
}");

            Core.ServiceHub.Instance.LoadModules(log, "Modules");

            if (context.GetConfig("services") == null)
                throw new Exception("Json Config is not valid");

            log.Info("ServiceHub running...");
            
            var servicesCount = context.GetConfig("services")?.Items.Count() ?? 0;

            for (int i = 0; i<servicesCount; i++)
            {
                context.SetNode($"services:{i}");
                Core.ServiceHub.Instance.InitModule(log, context);
                context.ResetNode();
            }

            foreach (var item in Core.ServiceHub.Instance.ServicesInitialized)
                Core.ServiceHub.Instance.Start(item);

            log.Info("Press any key to stop...");
            Console.ReadKey();

            foreach (var item in Core.ServiceHub.Instance.ServicesInitialized)
                Core.ServiceHub.Instance.Stop(item);

            log.Info("ServiceHub stopping...");

            while (Core.ServiceHub.Instance.ServicesInitialized.Count > 0)
                Thread.Sleep(100);

            log.Info("ServiceHub stopped.");
        }
    }
}
