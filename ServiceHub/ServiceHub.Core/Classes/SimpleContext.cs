using ServiceHub.Contracts.Interfaces;
using System.Text.Json;

namespace ServiceHub.Core.Classes
{
    public class SimpleContext : IConfigContext
    {
        internal ConfigNode Root { get; } = new();

        public string? Value
        {
            get
            {
                if (Root == null)
                    return null;
                return Root.Value;
            }
        }

        public IConfigContext[] Items { get => Root.Items.Select(x => new SimpleContext(x)).ToArray(); }

        public SimpleContext() { }
        internal SimpleContext(ConfigNode root) { Root = root; }

        public void LoadJson(string json)
        {
            using var doc = JsonDocument.Parse(json);
            LoadElement(Root, doc.RootElement);
        }

        private void LoadElement(ConfigNode node, JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var prop in element.EnumerateObject())
                    {
                        var child = node.GetOrAdd(prop.Name);
                        LoadElement(child, prop.Value);
                    }
                    break;

                case JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                    {
                        var child = node.AddItem();
                        LoadElement(child, item);
                    }
                    break;

                case JsonValueKind.String:
                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                    node.Value = element.ToString();
                    break;
            }
        }

        public string? Get(string path) => Traverse(path)?.Value;

        public IConfigContext? GetConfig(string path) => Traverse(path);

        private IConfigContext? Traverse(string path)
        {
            var parts = path.Split(':', StringSplitOptions.RemoveEmptyEntries);
            ConfigNode current = Root;

            foreach (var part in parts)
            {
                if (int.TryParse(part, out var index))
                {
                    if (current.Items.Count <= index)
                        return null;
                    current = current.Items[index];
                }
                else if (!current.Children.TryGetValue(part, out current))
                    return null;
            }

            return new SimpleContext(current);
        }
    }
}
