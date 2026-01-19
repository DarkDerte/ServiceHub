using ServiceHub.Contracts;

namespace ServiceHub.Host
{
    internal class SimpleContext : IServiceContext
    {
        public void Log(string message)
        {
            Console.WriteLine($"[LOG] {message}");
        }
    }
}
