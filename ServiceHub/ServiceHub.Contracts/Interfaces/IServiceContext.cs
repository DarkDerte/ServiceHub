using ServiceHub.Contracts.Classes;
using System.Text.Json;

namespace ServiceHub.Contracts.Interfaces
{
    public interface IServiceContext
    {
        string? Value { get; }

        void SetNode(string path);

        IServiceContext[] Items { get; }

        void LoadJson(string json);

        string? Get(string path);

        IServiceContext? GetConfig(string path);
    }
}
