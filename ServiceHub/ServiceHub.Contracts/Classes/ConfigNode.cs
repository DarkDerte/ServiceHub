namespace ServiceHub.Contracts.Classes
{
    public sealed class ConfigNode
    {
        private readonly Dictionary<string, ConfigNode> _children = new();
        private string? _value;

        public string? Value
        {
            get => _value;
            set => _value = value;
        }

        public IReadOnlyDictionary<string, ConfigNode> Children => _children;

        public ConfigNode GetOrAdd(string key)
        {
            if (!_children.TryGetValue(key, out var node))
            {
                node = new ConfigNode();
                _children[key] = node;
            }
            return node;
        }

        public bool TryGet(string key, out ConfigNode node) => _children.TryGetValue(key, out node!);
    }
}
