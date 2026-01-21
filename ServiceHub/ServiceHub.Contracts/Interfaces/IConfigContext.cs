namespace ServiceHub.Contracts.Interfaces
{
    public interface IConfigContext
    {
        string? Get(string path);

        IConfigContext? GetConfig(string path);

        string? Value { get; }

        IConfigContext[] Items { get; }
    }
}
