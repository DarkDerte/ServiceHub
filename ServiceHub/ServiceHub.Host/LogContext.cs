using ServiceHub.Contracts.Classes;
using ServiceHub.Contracts.Interfaces;

namespace ServiceHub.Host
{
    internal class LogContext : ILogContext
    {
        public void Info(string message) => Write(ConsoleColor.Blue, message);

        public void Warning(string message) => Write(ConsoleColor.Yellow, message);

        public void Error(string message) => Write(ConsoleColor.Red,message);

        private void Write(ConsoleColor Color, string message) 
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
