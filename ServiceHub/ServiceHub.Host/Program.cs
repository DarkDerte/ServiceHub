namespace ServiceHub.Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new SimpleContext();
            Core.ServiceHub.Instance.LoadModules("Modules", context);
            Core.ServiceHub.Instance.Start();

            Console.WriteLine("ServiceHub running. Press any key to stop...");
            Console.ReadKey();

            Core.ServiceHub.Instance.Stop();
            Console.WriteLine("ServiceHub stopped.");
        }
    }
}
