using ServiceHub.Contracts.Interfaces;
using System.Text.Json;
using System.Xml.Linq;

namespace ServiceHub.Core.Classes
{
    public class SimpleContext : IServiceContext
    {
        private ConfigNode Root { get; } = new ();

        private ConfigNode? Node { get; set; } = null;

        public string? Value
        {
            get
            {
                if (Node == null)
                    return Root.Value;
                return Node.Value;
            }
        }

        public void SetNode(string path)
        {
            if (Root == null)
                return;
            Node = Traverse(path);
        }

        public IServiceContext[] Items { get => (Node ?? Root).Items.Select(x => new SimpleContext(x)).ToArray(); }

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

        public IServiceContext? GetConfig(string path) 
        { 
            var data = Traverse(path); 
            return data == null ? null : (IServiceContext) new SimpleContext(data);
        }

        private ConfigNode? Traverse(string path)
        {
            var parts = path.Split(':', StringSplitOptions.RemoveEmptyEntries);
            ConfigNode? current = Node ?? Root;

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

            return current;
        }
    }
}
