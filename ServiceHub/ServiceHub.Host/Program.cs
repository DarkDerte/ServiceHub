namespace ServiceHub.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new SimpleContext();
            Core.ServiceHub.Instance.LoadModules("Modules", context);
            Core.ServiceHub.Instance.InitModule(Core.ServiceHub.Instance.Modules.First(), "test", context);
            Core.ServiceHub.Instance.Start("test");

            Console.WriteLine("ServiceHub running. Press any key to stop...");
            Console.ReadKey();

            Core.ServiceHub.Instance.Stop("test");

            Console.WriteLine("ServiceHub stopping.");

            while (Core.ServiceHub.Instance.ServicesInitialized.Count > 0)
                Thread.Sleep(100);

            Console.WriteLine("ServiceHub stopped.");
        }
    }
}
