namespace ServiceHub.Core.Classes
{
    internal class ConfigNode
    {
        private readonly Dictionary<string, ConfigNode> _children = new();
        private readonly List<ConfigNode> _items = new();

        public string? Value { get; set; }

        public IReadOnlyDictionary<string, ConfigNode> Children => _children;
        public IReadOnlyList<ConfigNode> Items => _items;

        public bool IsArray => _items.Count > 0;

        public ConfigNode GetOrAdd(string key)
        {
            if (!_children.TryGetValue(key, out var node))
            {
                node = new ConfigNode();
                _children[key] = node;
            }
            return node;
        }

        public ConfigNode AddItem()
        {
            var node = new ConfigNode();
            _items.Add(node);
            return node;
        }
    }
}
