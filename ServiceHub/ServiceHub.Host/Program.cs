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
    }
  ]
}");

            Core.ServiceHub.Instance.LoadModules(log, "Modules");

            if (context.GetConfig("services") == null)
                throw new Exception("Json Config is not valid");

            log.Info("ServiceHub running...");
            foreach (var localContext in context.GetConfig("services").Items)
            {
                Core.ServiceHub.Instance.InitModule(log, localContext);
                Core.ServiceHub.Instance.Start(localContext.Get("name"));

            }
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
